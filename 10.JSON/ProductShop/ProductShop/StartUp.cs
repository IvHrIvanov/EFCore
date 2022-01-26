using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using NHibernate.Cfg.ConfigurationSchema;
using ProductShop.Data;
using ProductShop.Datasets.Dtos.InputModels;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        private static IMapper mapper;
        public static void Main(string[] args)
        {
            ProductShopContext context = new ProductShopContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            string path = @"C:\Users\Ivan\EFCore\EFCore\10.JSON\ProductShop\ProductShop\Datasets\users.json";
            string pathProducts = @"C:\Users\Ivan\EFCore\EFCore\10.JSON\ProductShop\ProductShop\Datasets\products.json";
            var jsonStringReader = File.ReadAllText(path);
            var jsonStringReaderProducts = File.ReadAllText(pathProducts);
            string result = ImportUsers(context, jsonStringReader);
            string resultProducts = ImportProducts(context, jsonStringReaderProducts);

        }
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            IEnumerable<UserInputDto> users = JsonConvert.DeserializeObject<IEnumerable<UserInputDto>>(inputJson);

            MappConfiguration();

            var mappedUsers = mapper.Map<IEnumerable<User>>(users);

            context.Users.AddRange(mappedUsers);
            context.SaveChanges();
            return $"Successfully imported {mappedUsers.Count()}";
        }

        private static void MappConfiguration()
        {
            MapperConfiguration mappingConfiguration = new MapperConfiguration(x =>
            {
                x.AddProfile<ProductShopProfile>();
            });
            mapper = new Mapper(mappingConfiguration);
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            IEnumerable<Product> products = JsonConvert.DeserializeObject<IEnumerable<Product>>(inputJson);
            MappConfiguration();
            context.Products.AddRange(products);
            context.SaveChanges();
            return $"Successfully imported {products.Count()}"; ;
        }
    }
}