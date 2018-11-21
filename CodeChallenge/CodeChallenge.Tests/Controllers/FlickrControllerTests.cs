using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web.Http;
using System.Xml.Linq;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeChallenge;
using CodeChallenge.Controllers;
using CodeChallenge.Models;

namespace CodeChallenge.Tests.Controllers {
  [TestClass]
  public class FlickrControllerTests {
    [TestMethod]
    public void GetFlickrFeed_EmptyQueryValue_ReturnsFlickerImagesWithEmptyList() {
      // Arrange
      var controller = new FlickrController(new MockSuccessRestRequestAndFormat());

      // Act
      var result = controller.GetFlickrFeed(string.Empty);

      // Assert
      Assert.IsNotNull(result);
      Assert.IsTrue(result.Images.Count() == 0);
    }

    [TestMethod]
    public void GetFlickrFeed_ValidQuery_ReturnsPopulatedImagesList() {
      // Arrange
      var controller = new FlickrController(new MockSuccessRestRequestAndFormat());

      // Act
      var result = controller.GetFlickrFeed("Lucky Day");

      // Assert
      Assert.IsNotNull(result);
      Assert.IsTrue(result.Images.Count() > 0);
    }

    [TestMethod]
    public void GetFlickrFeed_NullResponse_ReturnsError() {
      // Arrange
      var controller = new FlickrController(new MockNullRestRequestResponseAndFailFormat());

      // Act
      var result = controller.GetFlickrFeed("Lucky Day");

      // Assert
      Assert.IsNotNull(result);
      Assert.IsTrue(result.IsError);
    }

    [TestMethod]
    public void GetFlickrFeed_FailFormat_ReturnsEmptyImagesList() {
      // Arrange
      var controller = new FlickrController(new MockSuccessRestRequestAndFailFormat());

      // Act
      var result = controller.GetFlickrFeed("Lucky Day");

      // Assert
      Assert.IsNotNull(result);
      Assert.IsTrue(result.Images.Count() == 0);
    }

    [TestMethod]
    public void GetFlickrFeed_FlickrResponseError_RelaysSameError() {
      // Arrange
      var controller = new FlickrController(new MockFailRestRequestAndFormat());

      // Act
      var result = controller.GetFlickrFeed("Lucky Day");

      // Assert
      Assert.IsNotNull(result);
      Assert.IsTrue(result.IsError);
      Assert.IsTrue(result.Images.Count() == 0);
    }

    #region Mock Classes
    public class MockSuccessRestRequestAndFormat : IRequest {
      public IResponse Get(string url) {
        var mockXml = new XElement("Mock");
        mockXml.Add(new XElement("Test", "Test"));

        return new MockResponseClass() {
          StatusCode = HttpStatusCode.OK,
          StatusMessage = HttpStatusCode.OK.ToString(),
          Xml = mockXml
        };
      }

      public IResponse FormatXmlToJson(string querry, XElement xml) {
        return new FlickrImages("query") {
          Images = new List<FlickrData>() {
            new FlickrData() {
              Title = Auxiliary.TITLE,
              Image = Auxiliary.HREF,
              PublishedDate = Auxiliary.PUBLISHED
            }
          }
        };
      }
    }

    public class MockNullRestRequestResponseAndFailFormat : IRequest {
      public IResponse Get(string url) {
        return null;
      }

      public IResponse FormatXmlToJson(string querry, XElement xml) {
        return new FlickrImages("query");
      }
    }

    public class MockSuccessRestRequestAndFailFormat : IRequest {
      public IResponse Get(string url) {
        var mockXml = new XElement("Mock");
        mockXml.Add(new XElement("Test", "Test"));

        return new MockResponseClass() {
          StatusCode = HttpStatusCode.OK,
          StatusMessage = HttpStatusCode.OK.ToString(),
          Xml = mockXml
        };
      }

      public IResponse FormatXmlToJson(string querry, XElement xml) {
        return new FlickrImages("query");
      }
    }

    public class MockFailRestRequestAndFormat : IRequest {
      public IResponse Get(string url) {
        return new MockResponseClass(new Exception()) {
          StatusCode = HttpStatusCode.BadRequest,
          StatusMessage = HttpStatusCode.BadRequest.ToString()
        };
      }

      public IResponse FormatXmlToJson(string querry, XElement xml) {
        return new FlickrImages("query");
      }
    }
    #endregion
  }
}
