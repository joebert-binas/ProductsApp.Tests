
using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Xunit;

namespace ProductsApp.Tests
{

    public class ProductsAppShould
    {
        [Fact]
        public void When_Add_Null_Product_Then_Should_Throw_ArgumentNullException() {

            //Arrange 
            var products = new Products();

            //Act and Assert
            Assert.Throws<ArgumentNullException>(() => products.AddNew(null));

        }


        [Fact]
        public void When_Add_New_Products_Then_Should_Return_ExpectedResult()
        {

            //Arrange 
            var products = new Products();
            var fixture = new Fixture();

            var product1 = fixture.Build<Product>().Without(x => x.IsSold).Create();
            var product2 = fixture.Build<Product>().Without(x => x.IsSold).Create();
            var product3 = fixture.Build<Product>().Without(x => x.IsSold).Create(); 

            //Act 
            products.AddNew(product1);
            products.AddNew(product2);
            products.AddNew(product3);

            //Assert
            Assert.True(products.Items.Any());
            Assert.Equal(3, products.Items.Count());

        }

        [Fact]
        public void When_Add_Null_ProductName_Then_Should_Throw_NameRequiredException()
        {

            //Arrange 
            var products = new Products();
            var newProduct = new Product();

            //Act and Assert
            Assert.Throws<NameRequiredException>(() => newProduct.Validate());

        }
    }

    internal class Products
    {
        private readonly List<Product> _products = new List<Product>();

        public IEnumerable<Product> Items => _products.Where(t => !t.IsSold);

        public void AddNew(Product product)
        {
            product = product ??
                throw new ArgumentNullException();
            product.Validate();
            _products.Add(product);
        }

        public void Sold(Product product)
        {
            product.IsSold = true;
        }

    }

    internal class Product
    {
        public bool IsSold { get; set; }
        public string Name { get; set; }

        internal void Validate()
        {
            Name = Name ??
                throw new NameRequiredException();
        }

    }

    [Serializable]
    internal class NameRequiredException : Exception
    {
        public NameRequiredException() { /* ... */ }

        public NameRequiredException(string message) : base(message) { /* ... */ }

        public NameRequiredException(string message, Exception innerException) : base(message, innerException) { /* ... */ }

        protected NameRequiredException(SerializationInfo info, StreamingContext context) : base(info, context) { /* ... */ }
    }
}
 