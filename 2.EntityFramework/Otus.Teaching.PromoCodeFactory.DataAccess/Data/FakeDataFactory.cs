﻿using System;
using System.Collections.Generic;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Data
{
    public static class FakeDataFactory
    {
        public static List<Role> Roles => new List<Role>()
        {
            new Role()
            {
                // Id = Guid.Parse("53729686-a368-4eeb-8bfa-cc69b6050d02"),
                Name = "Admin",
                Description = "Администратор",
            },
            new Role()
            {
                // Id = Guid.Parse("b0ae7aac-5493-45cd-ad16-87426a5e7665"),
                Name = "PartnerManager",
                Description = "Партнерский менеджер"
            }
        };

        public static List<Preference> Preferences => new List<Preference>()
        {
            new Preference()
            {
                // Id = Guid.Parse("ef7f299f-92d7-459f-896e-078ed53ef99c"),
                Name = "Театр",
            },
            new Preference()
            {
                // Id = Guid.Parse("c4bda62e-fc74-4256-a956-4760b3858cbd"),
                Name = "Семья",
            },
            new Preference()
            {
                // Id = Guid.Parse("76324c47-68d2-472d-abb8-33cfa8cc0c84"),
                Name = "Дети",
            }
        };

        public static List<Employee> Employees => new List<Employee>()
        {
            new Employee()
            {
                // Id = Guid.Parse("451533d5-d8d5-4a11-9c7b-eb9f14e1a32f"),
                Email = "owner@somemail.ru",
                FirstName = "Иван",
                LastName = "Сергеев",
                // Role = Roles.FirstOrDefault(x => x.Name == "Admin"),
                AppliedPromocodesCount = 5
            },
            new Employee()
            {
                // Id = Guid.Parse("f766e2bf-340a-46ea-bff3-f1700b435895"),
                Email = "andreev@somemail.ru",
                FirstName = "Петр",
                LastName = "Андреев",
                // Role = Roles.FirstOrDefault(x => x.Name == "PartnerManager"),
                AppliedPromocodesCount = 10
            },
        };

        public static List<PromoCode> PromoCodes => new List<PromoCode>()
        {
            new PromoCode()
            {
                // Id = Guid.Parse("451933d5-d8d5-4a11-988b-eb9f14e1a32f"),
                Code= "1-456",
                ServiceInfo = "service info 1",
                BeginDate = DateTime.Now,
                EndDate = DateTime.Parse("10-01-2025"),
                PartnerName = "Otus",
                PartnerManager = null!,
                Preference = null!
            },
            new PromoCode()
            {
                // Id = Guid.Parse("f766e2bf-840a-46ea-b883-f1700b435895"),
                Code= "2-456",
                ServiceInfo = "service info 1",
                BeginDate = DateTime.Now,
                EndDate = DateTime.Parse("10-01-2025"),
                PartnerName = "Otus",
                PartnerManager = null!,
                Preference = null!
            },
        };

        public static List<Customer> Customers
        {
            get
            {
                var customers = new List<Customer>()
                {
                    new Customer()
                    {
                        Email = "ivan_sergeev@mail.ru",
                        FirstName = "Иван",
                        LastName = "Петров",
                        //TODO: Добавить предзаполненный список предпочтений
                        Preferences = null!,
                        PromoCodes = null!
                    },
                    new Customer()
                    {
                        Email = "ivan_sergeev@mail.ru",
                        FirstName = "Алекс",
                        LastName = "Алексеев",
                        //TODO: Добавить предзаполненный список предпочтений
                        Preferences = null!,
                        PromoCodes = null!
                    }
                };

                return customers;
            }
        }
    }
}