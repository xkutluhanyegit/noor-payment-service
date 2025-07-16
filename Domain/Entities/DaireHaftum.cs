using System;
using System.Collections.Generic;
using Domain.Interfaces.Entity;

namespace Domain.Entities;

/// <summary>
/// Daire Hafta
/// </summary>
public partial class DaireHaftum:IEntity
{
    public int Id { get; set; }

    /// <summary>
    /// Created by
    /// </summary>
    public int? CreateUid { get; set; }

    /// <summary>
    /// Last Updated by
    /// </summary>
    public int? WriteUid { get; set; }

    /// <summary>
    /// Daire Hafta Kimlik
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Daire Kimlik
    /// </summary>
    public string? DaireKimlik { get; set; }

    /// <summary>
    /// BBSN NO
    /// </summary>
    public string? BbsnNo { get; set; }

    /// <summary>
    /// CRM Durumu
    /// </summary>
    public string? CrmDurumu { get; set; }

    /// <summary>
    /// Created on
    /// </summary>
    public DateTime? CreateDate { get; set; }

    /// <summary>
    /// Last Updated on
    /// </summary>
    public DateTime? WriteDate { get; set; }

    public virtual ICollection<YildatOdeme> YildatOdemes { get; set; } = new List<YildatOdeme>();

    public virtual ICollection<Yildat> Yildats { get; set; } = new List<Yildat>();
}
