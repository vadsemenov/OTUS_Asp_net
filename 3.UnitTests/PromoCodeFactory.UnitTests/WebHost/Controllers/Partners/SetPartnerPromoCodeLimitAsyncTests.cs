using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Data;
using PromoCodeFactory.WebHost.Controllers;
using PromoCodeFactory.WebHost.Models;
using Xunit;

namespace PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public class SetPartnerPromoCodeLimitAsyncTests
    {
        //TODO: Add Unit Tests
        private readonly Mock<IRepository<Partner>> _partnersRepositoryMock;
        private readonly PartnersController _partnersController;

        public SetPartnerPromoCodeLimitAsyncTests()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _partnersRepositoryMock = fixture.Freeze<Mock<IRepository<Partner>>>();
            _partnersController = fixture.Build<PartnersController>().OmitAutoProperties().Create();
        }

        //1. Если партнер не найден, то также нужно выдать ошибку 404;
        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_PartnerIsNotFound_ReturnsNotFound()
        {
            // Arrange
            var partnerId = Guid.Parse("def47943-7aaf-44a1-ae21-05aa4948b179");
            Partner partner = null;

            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);

            var promoCodeLimitRequest = new SetPartnerPromoCodeLimitRequest
            {
                EndDate = DateTime.Now + TimeSpan.FromDays(2),
                Limit = 2
            };

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, promoCodeLimitRequest);

            // Assert
            result.Should().BeAssignableTo<NotFoundResult>();
        }

        //2. Если партнер заблокирован, то есть поле IsActive=false в классе Partner, то также нужно выдать ошибку 400;
        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_PartnerIsNotActive_ReturnsBadRequest()
        {
            // Arrange
            var partner = PartnerFactory.CreatePartner(isActive: false);
            var partnerId = partner.Id;

            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);

            var promoCodeLimitRequest = new SetPartnerPromoCodeLimitRequest
            {
                EndDate = DateTime.Now + TimeSpan.FromDays(2),
                Limit = 2
            };

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, promoCodeLimitRequest);

            // Assert
            result.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        //3.Если партнеру выставляется лимит, то мы должны обнулить количество промокодов,
        //которые партнер выдал NumberIssuedPromoCodes,
        //если лимит закончился, то количество не обнуляется;
        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_PartnerIsHaveLimit_ReturnsNumberIssuedPromoCodesEqualsZero()
        {
            // Arrange
            var partner = PartnerFactory.CreatePartner(isActive: true, numberIssuedPromoCodes: 5, cancelDate: null);
            var partnerId = partner.Id;

            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);

            var promoCodeLimitRequest = new SetPartnerPromoCodeLimitRequest
            {
                EndDate = DateTime.Now + TimeSpan.FromDays(2),
                Limit = 2
            };

            // Act
            await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, promoCodeLimitRequest);

            // Assert
            partner.NumberIssuedPromoCodes.Should().Be(0);
        }

        //4. При установке лимита нужно отключить предыдущий лимит;
        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_PartnerIsNotHaveCancelDate_ReturnsCancelDateIsNotNull()
        {
            // Arrange
            var partner = PartnerFactory.CreatePartner(isActive: true, cancelDate: null);
            var partnerId = partner.Id;

            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);

            var activeLimit = partner.PartnerLimits.FirstOrDefault(x =>
                !x.CancelDate.HasValue);

            var promoCodeLimitRequest = new SetPartnerPromoCodeLimitRequest
            {
                EndDate = DateTime.Now + TimeSpan.FromDays(2),
                Limit = 2
            };

            // Act
            await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, promoCodeLimitRequest);

            // Assert
            activeLimit.CancelDate.Should().NotBeNull();
        }

        //5. Лимит должен быть больше 0;
        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_LimitEqualsZero_ReturnBadRequest()
        {
            // Arrange
            var partner = PartnerFactory.CreatePartner(isActive: true, cancelDate: null);
            var partnerId = partner.Id;

            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);

            var promoCodeLimitRequest = new SetPartnerPromoCodeLimitRequest
            {
                EndDate = DateTime.Now + TimeSpan.FromDays(2),
                Limit = 0
            };

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, promoCodeLimitRequest);

            // Assert
            result.Should().BeAssignableTo<BadRequestObjectResult>()
                .Which.Value.Should().Be("Лимит должен быть больше 0");
        }


        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_ConditionыPassedSuccess_PartnerPromoCodeLimitCreteAndSave()
        {
            // Arrange
            var partner = PartnerFactory.CreatePartner(isActive: true, cancelDate: null);
            var partnerId = partner.Id;

            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);

            var promoCodeLimitRequest = new SetPartnerPromoCodeLimitRequest
            {
                EndDate = DateTime.Now + TimeSpan.FromDays(2),
                Limit = 100
            };

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, promoCodeLimitRequest);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();

            partner.PartnerLimits.Should()
                .Contain(x => x.Limit == 100
                              && x.EndDate == promoCodeLimitRequest.EndDate
                              && x.PartnerId == partner.Id);
            
            _partnersRepositoryMock
                .Verify(repo => repo.UpdateAsync(It.IsAny<Partner>()), Times.Once);
        }
    }
}