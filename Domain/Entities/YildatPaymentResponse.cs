using System;
using System.Collections.Generic;
using System.Net;
using Domain.Interfaces.Entity;

namespace Domain.Entities;

public partial class YildatPaymentResponse:IEntity
{
    public int Id { get; set; }

    public long? Clientid { get; set; }

    public string? Oid { get; set; }

    public string? Errmsg { get; set; }

    public decimal? Amount { get; set; }

    public int? Currency { get; set; }

    public string? Procreturncode { get; set; }

    public string? Authcode { get; set; }

    public string? Storetype { get; set; }

    public int? Mdstatus { get; set; }

    public string? Hash { get; set; }

    public string? Response { get; set; }

    public IPAddress? Clientip { get; set; }

    public string? Hashalgorithm { get; set; }

    public string? Transid { get; set; }
}
