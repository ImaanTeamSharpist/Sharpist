﻿namespace Sharpist.Server.API.Models;

public class Response
{
    public int Code { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }
}
