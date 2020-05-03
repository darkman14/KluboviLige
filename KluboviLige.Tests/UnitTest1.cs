using KluboviLige.Controllers;
using KluboviLige.Interfaces;
using KluboviLige.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace KluboviLige.Tests
{
    [TestClass]
    public class UnitTest1
    {

        //Test GetById vraća objekat i 200 OK
        [TestMethod]
        public void GetReturnsProductWithSameId()
        {
            // Arrange
            var mockRepository = new Mock<IClubRepository>();
            mockRepository.Setup(x => x.GetById(11)).Returns(new Club { Id = 11 });

            var controller = new KluboviController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Get(11);
            var contentResult = actionResult as OkNegotiatedContentResult<Club>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(11, contentResult.Content.Id);
        }


        //Test GetById vraća 400 NotFound
        [TestMethod]
        public void GetReturnsNotFound()
        {
            // Arrange
            var mockRepository = new Mock<IClubRepository>();
            var controller = new KluboviController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Get(10);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        //Test POST vraća 201 Created i objekat
        [TestMethod]
        public void PostMethodSetsLocationHeader()
        {
            // Arrange
            var mockRepository = new Mock<IClubRepository>();
            var controller = new KluboviController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Post(new Club { Id = 10, Name = "KlubTest", Town="Grad", YearOfEst=1999, Price=11, LeagueId=1 });
            var createdResult = actionResult as CreatedAtRouteNegotiatedContentResult<Club>;

            // Assert
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("DefaultApi", createdResult.RouteName);
            Assert.AreEqual(10, createdResult.RouteValues["id"]);
        }

        //Test DELETE vraća 200 OK
        [TestMethod]
        public void DeleteReturnsOk()
        {
            // Arrange
            var mockRepository = new Mock<IClubRepository>();
            mockRepository.Setup(x => x.GetById(10)).Returns(new Club{ Id = 10 });
            var controller = new KluboviController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Delete(10);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkResult));
        }

    }
}
