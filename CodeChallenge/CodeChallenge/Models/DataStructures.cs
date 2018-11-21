using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CodeChallenge.Models {
  public class Auxiliary {
    public const string ENTRY = "entry";
    public const string TITLE = "title";
    public const string PUBLISHED = "published";
    public const string LINK = "link";
    public const string REL = "rel";
    public const string ENCLOSURE = "enclosure";
    public const string HREF = "href";
  }

  public class FlickrImages : Response {
    public string Querry;
    public List<FlickrData> Images;

    public FlickrImages(string query) : base() {
      Querry = query;
      Images = new List<FlickrData>();
    }
    public FlickrImages(
      string query,
      Exception ex,
      HttpStatusCode statusCode,
      string statusMessage = null
    ) : base(ex, statusCode, statusMessage) {
      Querry = query;
      Images = new List<FlickrData>();
    }
  }

  public class FlickrData {
    public string Title { get; set; }
    public string Image { get; set; }
    public string PublishedDate { get; set; }
  }

  public class FlickrFeedResponse : Response {
    public FlickrFeedResponse() : base() {

    }
    public FlickrFeedResponse(
      Exception ex,
      HttpStatusCode statusCode,
      string statusMessage = null
    ) : base(ex, statusCode, statusMessage) {

    }
  }

  public abstract class Response : IResponse {
    public HttpStatusCode StatusCode { get; set; }
    public string StatusMessage { get; set; }
    public XElement Xml { get; set; }
    public Exception Ex { get; }
    public bool IsError {
      get {
        return Ex != null;
      }
    }

    public Response() {
      StatusCode = HttpStatusCode.OK;
      StatusMessage = HttpStatusCode.OK.ToString();
    }
    public Response(Exception ex, HttpStatusCode statusCode, string statusMessage) {
      Ex = ex;
      StatusCode = statusCode;
      StatusMessage = statusMessage;
      if (string.IsNullOrEmpty(StatusMessage)) {
        StatusMessage = ex.Message;
      }
    }
  }
}