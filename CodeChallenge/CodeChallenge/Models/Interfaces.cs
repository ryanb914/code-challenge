using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;

namespace CodeChallenge.Models {
  public interface IRequest {
    IResponse Get(string url);
    IResponse FormatXmlToJson(string querry, XElement xml);
  }

  public interface IResponse {
    HttpStatusCode StatusCode { get; set; }
    string StatusMessage { get; set; }
    XElement Xml { get; set; }
    Exception Ex { get; }
    bool IsError { get; }
  }
}