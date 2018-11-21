using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeChallenge;
using CodeChallenge.Controllers;
using CodeChallenge.Models;

namespace CodeChallenge.Tests.Controllers {
  #region Response Tests
  [TestClass]
  public class ResponseTests {
    [TestMethod]
    public void Response_EmptyConstructor_StatusOK() {
      // Arrange
      var mockResponseClass = new MockResponseClass();

      // Act


      // Assert
      Assert.IsTrue(mockResponseClass.StatusCode == HttpStatusCode.OK);
    }

    [TestMethod]
    public void Response_ValideInput_XmlValueSet() {
      // Arrange
      XElement mockXml = new XElement("Mock");
      mockXml.Add(new XElement("MockElement", "MockValue"));
      var mockResponseClass = new MockResponseClass() {
        Xml = mockXml
      };

      // Act


      // Assert
      Assert.IsNotNull(mockResponseClass.Xml);
      Assert.IsFalse(mockResponseClass.Xml.IsEmpty);
    }

    [TestMethod]
    public void Response_NullInput_XmlIsNull() {
      // Arrange
      var mockResponseClass = new MockResponseClass() {
        Xml = null
      };

      // Act


      // Assert
      Assert.IsNull(mockResponseClass.Xml);
    }

    [TestMethod]
    public void Response_ExceptionConstructor_ExceptionAndIsErrorSet() {
      // Arrange
      var mockResponseClass = new MockResponseClass(new Exception());

      // Act


      // Assert
      Assert.IsTrue(mockResponseClass.IsError);
      Assert.IsTrue(mockResponseClass.Ex != null);
    }

    [TestMethod]
    public void Response_ExceptionConstructor_ExceptionProvidedStatusCodeAndMessage() {
      // Arrange
      const string ERROR_MSG = "error message";
      const HttpStatusCode ERROR_CODE = HttpStatusCode.BadRequest;
      var mockResponseClass = new MockResponseClass(
        new Exception(),
        ERROR_CODE,
        ERROR_MSG
      );

      // Act


      // Assert
      Assert.IsTrue(mockResponseClass.IsError);
      Assert.IsTrue(mockResponseClass.Ex != null);
      Assert.IsTrue(mockResponseClass.StatusCode == ERROR_CODE);
      Assert.AreEqual(mockResponseClass.StatusMessage, ERROR_MSG);
    }
  }

  class MockResponseClass : Response {
    public MockResponseClass() : base() {

    }
    public MockResponseClass(Exception ex) :
      base(ex, default(HttpStatusCode), string.Empty) {

    }
    public MockResponseClass(Exception ex, HttpStatusCode statusCode, string statusMessage) :
      base(ex, statusCode, statusMessage) {

    }
  }
  class MockImageClass : Response {
    public MockImageClass() : base() {

    }
    public MockImageClass(Exception ex) :
      base(ex, default(HttpStatusCode), string.Empty) {

    }
    public MockImageClass(Exception ex, HttpStatusCode statusCode, string statusMessage) :
      base(ex, statusCode, statusMessage) {

    }
  }
  #endregion
  #region FlickrFeedResponse Tests
  [TestClass]
  public class FlickrFeedResponseTests {
    [TestMethod]
    public void FlickrFeedResponse_EmptyConstructor_StatusOK() {
      // Arrange
      var mockFlickrFeedResponse = new FlickrFeedResponse();

      // Act


      // Assert
      Assert.IsTrue(mockFlickrFeedResponse.StatusCode == HttpStatusCode.OK);
    }

    [TestMethod]
    public void FlickrFeedResponse_ExceptionConstructor_ExceptionProvidedStatusCodeAndMessage() {
      // Arrange
      const string ERROR_MSG = "error message";
      const HttpStatusCode ERROR_CODE = HttpStatusCode.BadRequest;
      var mockFlickrFeedResponse = new FlickrFeedResponse(
        new Exception(),
        ERROR_CODE,
        ERROR_MSG
      );

      // Act


      // Assert
      Assert.IsTrue(mockFlickrFeedResponse.IsError);
      Assert.IsTrue(mockFlickrFeedResponse.Ex != null);
      Assert.IsTrue(mockFlickrFeedResponse.StatusCode == ERROR_CODE);
      Assert.AreEqual(mockFlickrFeedResponse.StatusMessage, ERROR_MSG);
    }
  }
  #endregion
  #region FlickrImages Tests
  [TestClass]
  public class FlickrImagesTests {
    [TestMethod]
    public void FlickrImages_ConstructorWithValidQueryInput_StatusOKAndQueryMatchAndEmptyList() {
      // Arrange
      const string QUERY = "test";
      var mockFlickrImages = new FlickrImages(QUERY);

      // Act


      // Assert
      Assert.IsTrue(mockFlickrImages.StatusCode == HttpStatusCode.OK);
      Assert.AreEqual(mockFlickrImages.Querry, QUERY);
      Assert.IsNotNull(mockFlickrImages.Images);
      Assert.IsTrue(mockFlickrImages.Images.Count() == 0);
    }

    [TestMethod]
    public void FlickrImages_ExceptionConstructor_ExceptionProvidedStatusCodeAndMessage() {
      // Arrange
      const string QUERY = "test";
      const string ERROR_MSG = "error message";
      const HttpStatusCode ERROR_CODE = HttpStatusCode.BadRequest;
      var mockFlickrImages = new FlickrImages(
        QUERY,
        new Exception(),
        ERROR_CODE,
        ERROR_MSG
      );

      // Act


      // Assert
      Assert.IsTrue(mockFlickrImages.IsError);
      Assert.IsTrue(mockFlickrImages.Ex != null);
      Assert.AreEqual(mockFlickrImages.Querry, QUERY);
      Assert.IsTrue(mockFlickrImages.StatusCode == ERROR_CODE);
      Assert.AreEqual(mockFlickrImages.StatusMessage, ERROR_MSG);
    }

    [TestMethod]
    public void FlickrData_ValidInput_ValuesMatch() {
      // Arrange
      const string TITLE = "mock title";
      const string DATE = "mock date";
      const string IMAGE = "mock image";
      var mockFlickrData = new FlickrData() {
        Title = TITLE,
        PublishedDate = DATE,
        Image = IMAGE
      };

      // Act


      // Assert
      Assert.AreEqual(mockFlickrData.Title, TITLE);
      Assert.AreEqual(mockFlickrData.PublishedDate, DATE);
      Assert.AreEqual(mockFlickrData.Image, IMAGE);
    }

    [TestMethod]
    public void FlickrData_NullValues_ValuesAReEmpty() {
      // Arrange
      var mockFlickrData = new FlickrData() {
        Title = null,
        PublishedDate = null,
        Image = null
      };

      // Act


      // Assert
      Assert.AreEqual(mockFlickrData.Title, null);
      Assert.AreEqual(mockFlickrData.PublishedDate, null);
      Assert.AreEqual(mockFlickrData.Image, null);
    }
  }
  #endregion
}
