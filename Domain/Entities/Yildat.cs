using System;
using System.Collections.Generic;
using Domain.Interfaces.Entity;

namespace Domain.Entities;

/// <summary>
/// Yıldat Kaydı
/// </summary>
public partial class Yildat:IEntity
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
    /// Arı Sözleşme No
    /// </summary>
    public string? SozlesmeNo { get; set; }

    /// <summary>
    /// Müşteri
    /// </summary>
    public string? Musteri { get; set; }

    /// <summary>
    /// Satış Durumu
    /// </summary>
    public string? SatisDurumu { get; set; }

    /// <summary>
    /// Sözleşme Giriş Tarihi
    /// </summary>
    public DateOnly? GirisTarihi { get; set; }

    /// <summary>
    /// Başlangıç Tarihi
    /// </summary>
    public DateOnly? BaslangicTarihi { get; set; }

    /// <summary>
    /// Bitiş Tarihi
    /// </summary>
    public DateOnly? BitisTarihi { get; set; }

    /// <summary>
    /// Created on
    /// </summary>
    public DateTime? CreateDate { get; set; }

    /// <summary>
    /// Last Updated on
    /// </summary>
    public DateTime? WriteDate { get; set; }

    /// <summary>
    /// Yıldat Tutarı
    /// </summary>
    public double? YildatTutari { get; set; }

    /// <summary>
    /// Kurulum Bedeli
    /// </summary>
    public double? KurulumBedeli { get; set; }

    /// <summary>
    /// Gecikme Bedeli
    /// </summary>
    public double? GecikmeBedeli { get; set; }

    /// <summary>
    /// Gecikme Yüzdesi (%)
    /// </summary>
    public double? GecikmeYuzdesi { get; set; }

    /// <summary>
    /// Ödenen Tutar
    /// </summary>
    public double? OdenenTutar { get; set; }

    /// <summary>
    /// Statü
    /// </summary>
    public string? Statu { get; set; }

    /// <summary>
    /// Kalan Kurulum Tutarı
    /// </summary>
    public double? KalanKurulumTutari { get; set; }

    /// <summary>
    /// Kalan Yıldat Tutarı
    /// </summary>
    public double? KalanYildatTutari { get; set; }

    /// <summary>
    /// Yıl
    /// </summary>
    public string? Yil { get; set; }

    /// <summary>
    /// Daire Hafta Kimlik
    /// </summary>
    public int? DaireHaftaId { get; set; }

    /// <summary>
    /// Fazla Ödenen Tutar
    /// </summary>
    public double? FazlaOdenenTutar { get; set; }

    /// <summary>
    /// Toplam Ödeme Tutarı
    /// </summary>
    public double? ToplamOdemeTutari { get; set; }

    /// <summary>
    /// Ödenecek
    /// </summary>
    public double? KalanOdenecek { get; set; }

    /// <summary>
    /// Ev Tipi
    /// </summary>
    public string? EvTipi { get; set; }

    /// <summary>
    /// Blok No
    /// </summary>
    public string? BlokNo { get; set; }

    /// <summary>
    /// Giriş Adı
    /// </summary>
    public string? GirisAdi { get; set; }

    /// <summary>
    /// Cephe
    /// </summary>
    public string? Cephe { get; set; }

    /// <summary>
    /// Kat
    /// </summary>
    public string? Kat { get; set; }

    /// <summary>
    /// Mevsim
    /// </summary>
    public string? Mevsim { get; set; }

    /// <summary>
    /// Daire BB No
    /// </summary>
    public string? DaireBbNo { get; set; }

    /// <summary>
    /// Proje
    /// </summary>
    public string? Proje { get; set; }

    /// <summary>
    /// Ödeme Sistemi
    /// </summary>
    public string? OdemeSistemi { get; set; }

    /// <summary>
    /// Satış Tipi
    /// </summary>
    public string? SatisTipi { get; set; }

    /// <summary>
    /// Stok No
    /// </summary>
    public string? NavDaireNo { get; set; }

    /// <summary>
    /// IFS Kodu
    /// </summary>
    public string? IfsKodu { get; set; }

    /// <summary>
    /// Ödeme Planı
    /// </summary>
    public string? OdemePlani { get; set; }

    /// <summary>
    /// Daire Dönem
    /// </summary>
    public string? DaireDonem { get; set; }

    /// <summary>
    /// GYO Sözleşme No
    /// </summary>
    public string? GyoSozlesmeNo { get; set; }

    /// <summary>
    /// Güncel Satış Durumu
    /// </summary>
    public string? SatisDurumuGuncel { get; set; }

    /// <summary>
    /// Daire Brüt m²
    /// </summary>
    public double? DaireBrutM2 { get; set; }

    /// <summary>
    /// Genel Brüt m²
    /// </summary>
    public double? GenelBrutM2 { get; set; }

    /// <summary>
    /// TCKN
    /// </summary>
    public string? Tckn { get; set; }

    /// <summary>
    /// Telefon No
    /// </summary>
    public string? TelNo { get; set; }

    /// <summary>
    /// Yedek
    /// </summary>
    public string? Bos { get; set; }

    public virtual DaireHaftum? DaireHafta { get; set; }

    public virtual ICollection<YildatOdeme> YildatOdemes { get; set; } = new List<YildatOdeme>();
}
