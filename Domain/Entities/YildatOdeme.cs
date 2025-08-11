using System;
using System.Collections.Generic;
using Domain.Interfaces.Entity;

namespace Domain.Entities;

/// <summary>
/// Yıldat Ödeme Detayları
/// </summary>
public partial class YildatOdeme:IEntity
{
    public int Id { get; set; }

    /// <summary>
    /// Yıldat
    /// </summary>
    public int? YildatId { get; set; }

    /// <summary>
    /// Satır No
    /// </summary>
    public int? SatirNo { get; set; }

    /// <summary>
    /// Created by
    /// </summary>
    public int? CreateUid { get; set; }

    /// <summary>
    /// Last Updated by
    /// </summary>
    public int? WriteUid { get; set; }

    /// <summary>
    /// Hizmet Türü
    /// </summary>
    public string? HizmetTuru { get; set; }

    /// <summary>
    /// Ödeme Türü
    /// </summary>
    public string OdemeTuru { get; set; } = null!;

    /// <summary>
    /// Created on
    /// </summary>
    public DateTime? CreateDate { get; set; }

    /// <summary>
    /// Last Updated on
    /// </summary>
    public DateTime? WriteDate { get; set; }

    /// <summary>
    /// Ödeme Tutarı
    /// </summary>
    public double Tutar { get; set; }

    /// <summary>
    /// Ödeme Tarihi
    /// </summary>
    public DateOnly? OdemeTarihi { get; set; }

    /// <summary>
    /// Daire Hafta Kimlik
    /// </summary>
    public int? DaireHaftaId { get; set; }

    /// <summary>
    /// Tahakkuk Tutarı
    /// </summary>
    public double? TahTutar { get; set; }

    public virtual DaireHaftum? DaireHafta { get; set; }

    public virtual Yildat? Yildat { get; set; }
}
