using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Contexts;

public partial class Noor17Context : DbContext
{
    public Noor17Context()
    {
    }

    public Noor17Context(DbContextOptions<Noor17Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Yildat> Yildats { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=192.168.228.37;Port=5432;Database=NOOR17;Username=webadmin;Password=Sn569632");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("C")
            .HasPostgresExtension("pg_trgm");

        modelBuilder.Entity<Yildat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("yildat_pkey");

            entity.ToTable("yildat", tb => tb.HasComment("Yıldat Kaydı"));

            entity.HasIndex(e => e.Tckn, "yildat__tckn_index");

            entity.HasIndex(e => e.TelNo, "yildat__tel_no_index");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BaslangicTarihi)
                .HasComment("Başlangıç Tarihi")
                .HasColumnName("baslangic_tarihi");
            entity.Property(e => e.BitisTarihi)
                .HasComment("Bitiş Tarihi")
                .HasColumnName("bitis_tarihi");
            entity.Property(e => e.BlokNo)
                .HasComment("Blok No")
                .HasColumnType("character varying")
                .HasColumnName("blok_no");
            entity.Property(e => e.Bos)
                .HasComment("Yedek")
                .HasColumnType("character varying")
                .HasColumnName("bos");
            entity.Property(e => e.Cephe)
                .HasComment("Cephe")
                .HasColumnType("character varying")
                .HasColumnName("cephe");
            entity.Property(e => e.CreateDate)
                .HasComment("Created on")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("create_date");
            entity.Property(e => e.CreateUid)
                .HasComment("Created by")
                .HasColumnName("create_uid");
            entity.Property(e => e.DaireBbNo)
                .HasComment("Daire BB No")
                .HasColumnType("character varying")
                .HasColumnName("daire_bb_no");
            entity.Property(e => e.DaireBrutM2)
                .HasComment("Daire Brüt m²")
                .HasColumnName("daire_brut_m2");
            entity.Property(e => e.DaireDonem)
                .HasComment("Daire Dönem")
                .HasColumnType("character varying")
                .HasColumnName("daire_donem");
            entity.Property(e => e.DaireHaftaId)
                .HasComment("Daire Hafta Kimlik")
                .HasColumnName("daire_hafta_id");
            entity.Property(e => e.EvTipi)
                .HasComment("Ev Tipi")
                .HasColumnType("character varying")
                .HasColumnName("ev_tipi");
            entity.Property(e => e.FazlaOdenenTutar)
                .HasComment("Fazla Ödenen Tutar")
                .HasColumnName("fazla_odenen_tutar");
            entity.Property(e => e.GecikmeBedeli)
                .HasComment("Gecikme Bedeli")
                .HasColumnName("gecikme_bedeli");
            entity.Property(e => e.GecikmeYuzdesi)
                .HasComment("Gecikme Yüzdesi (%)")
                .HasColumnName("gecikme_yuzdesi");
            entity.Property(e => e.GenelBrutM2)
                .HasComment("Genel Brüt m²")
                .HasColumnName("genel_brut_m2");
            entity.Property(e => e.GirisAdi)
                .HasComment("Giriş Adı")
                .HasColumnType("character varying")
                .HasColumnName("giris_adi");
            entity.Property(e => e.GirisTarihi)
                .HasComment("Sözleşme Giriş Tarihi")
                .HasColumnName("giris_tarihi");
            entity.Property(e => e.GyoSozlesmeNo)
                .HasComment("GYO Sözleşme No")
                .HasColumnType("character varying")
                .HasColumnName("gyo_sozlesme_no");
            entity.Property(e => e.IfsKodu)
                .HasComment("IFS Kodu")
                .HasColumnType("character varying")
                .HasColumnName("ifs_kodu");
            entity.Property(e => e.KalanKurulumTutari)
                .HasComment("Kalan Kurulum Tutarı")
                .HasColumnName("kalan_kurulum_tutari");
            entity.Property(e => e.KalanOdenecek)
                .HasComment("Ödenecek")
                .HasColumnName("kalan_odenecek");
            entity.Property(e => e.KalanYildatTutari)
                .HasComment("Kalan Yıldat Tutarı")
                .HasColumnName("kalan_yildat_tutari");
            entity.Property(e => e.Kat)
                .HasComment("Kat")
                .HasColumnType("character varying")
                .HasColumnName("kat");
            entity.Property(e => e.KurulumBedeli)
                .HasComment("Kurulum Bedeli")
                .HasColumnName("kurulum_bedeli");
            entity.Property(e => e.Mevsim)
                .HasComment("Mevsim")
                .HasColumnType("character varying")
                .HasColumnName("mevsim");
            entity.Property(e => e.Musteri)
                .HasComment("Müşteri")
                .HasColumnType("character varying")
                .HasColumnName("musteri");
            entity.Property(e => e.NavDaireNo)
                .HasComment("Stok No")
                .HasColumnType("character varying")
                .HasColumnName("nav_daire_no");
            entity.Property(e => e.OdemePlani)
                .HasComment("Ödeme Planı")
                .HasColumnType("character varying")
                .HasColumnName("odeme_plani");
            entity.Property(e => e.OdemeSistemi)
                .HasComment("Ödeme Sistemi")
                .HasColumnType("character varying")
                .HasColumnName("odeme_sistemi");
            entity.Property(e => e.OdenenTutar)
                .HasComment("Ödenen Tutar")
                .HasColumnName("odenen_tutar");
            entity.Property(e => e.Proje)
                .HasComment("Proje")
                .HasColumnType("character varying")
                .HasColumnName("proje");
            entity.Property(e => e.SatisDurumu)
                .HasComment("Satış Durumu")
                .HasColumnType("character varying")
                .HasColumnName("satis_durumu");
            entity.Property(e => e.SatisDurumuGuncel)
                .HasComment("Güncel Satış Durumu")
                .HasColumnType("character varying")
                .HasColumnName("satis_durumu_guncel");
            entity.Property(e => e.SatisTipi)
                .HasComment("Satış Tipi")
                .HasColumnType("character varying")
                .HasColumnName("satis_tipi");
            entity.Property(e => e.SozlesmeNo)
                .HasComment("Arı Sözleşme No")
                .HasColumnType("character varying")
                .HasColumnName("sozlesme_no");
            entity.Property(e => e.Statu)
                .HasComment("Statü")
                .HasColumnType("character varying")
                .HasColumnName("statu");
            entity.Property(e => e.Tckn)
                .HasComment("TCKN")
                .HasColumnType("character varying")
                .HasColumnName("tckn");
            entity.Property(e => e.TelNo)
                .HasComment("Telefon No")
                .HasColumnType("character varying")
                .HasColumnName("tel_no");
            entity.Property(e => e.ToplamOdemeTutari)
                .HasComment("Toplam Ödeme Tutarı")
                .HasColumnName("toplam_odeme_tutari");
            entity.Property(e => e.WriteDate)
                .HasComment("Last Updated on")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("write_date");
            entity.Property(e => e.WriteUid)
                .HasComment("Last Updated by")
                .HasColumnName("write_uid");
            entity.Property(e => e.Yil)
                .HasComment("Yıl")
                .HasColumnType("character varying")
                .HasColumnName("yil");
            entity.Property(e => e.YildatTutari)
                .HasComment("Yıldat Tutarı")
                .HasColumnName("yildat_tutari");
        });
        modelBuilder.HasSequence("base_cache_signaling_assets");
        modelBuilder.HasSequence("base_cache_signaling_default");
        modelBuilder.HasSequence("base_cache_signaling_routing");
        modelBuilder.HasSequence("base_cache_signaling_templates");
        modelBuilder.HasSequence("base_registry_signaling");
        modelBuilder.HasSequence("ir_sequence_001");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
