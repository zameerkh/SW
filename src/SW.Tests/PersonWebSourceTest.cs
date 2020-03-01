using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SW.Api.Contracts;
using SW.Api.Models;
using SW.Api.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SW.Tests
{
    [TestClass]
    public class PersonWebSourceTest
    {
        [TestMethod]
        public async Task Test_GetById()
        {
            //Arrange
            IList<Person> moqData = new List<Person>()
            {
                new Person(){id = 1, age = 23,  First = "Bryan", last = "Reynolds", gender = "M"}
            }; ;

            var moqDataSource = new Mock<IGetPersons>();
            moqDataSource.Setup(x => x.GetPersons()).Returns(Task.FromResult(moqData

                ));
            var sut = new PersonWebSource(moqDataSource.Object);


            //Act
            var result = await sut.GetById(1);

            //Assert
            result.Should().NotBe(null);
            result.id.Should().Be(1);

        }
    }
}
