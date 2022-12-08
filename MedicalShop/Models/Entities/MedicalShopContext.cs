using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MedicalShop.Models.Entities
{
    public partial class MedicalShopContext : DbContext
    {
        public MedicalShopContext()
        {
        }

        public MedicalShopContext(DbContextOptions<MedicalShopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ChiNhanh> ChiNhanh { get; set; }
        public virtual DbSet<ChiTietPhieuNhap> ChiTietPhieuNhap { get; set; }
        public virtual DbSet<ChiTietPhieuXuat> ChiTietPhieuXuat { get; set; }
        public virtual DbSet<ChiTietTraNo> ChiTietTraNo { get; set; }
        public virtual DbSet<ChucNang> ChucNang { get; set; }
        public virtual DbSet<Dvbh> Dvbh { get; set; }
        public virtual DbSet<Dvt> Dvt { get; set; }
        public virtual DbSet<Dvvc> Dvvc { get; set; }
        public virtual DbSet<HangHoa> HangHoa { get; set; }
        public virtual DbSet<HhDvt> HhDvt { get; set; }
        public virtual DbSet<Hsx> Hsx { get; set; }
        public virtual DbSet<Httt> Httt { get; set; }
        public virtual DbSet<KhachHang> KhachHang { get; set; }
        public virtual DbSet<NganHang> NganHang { get; set; }
        public virtual DbSet<NganHangKh> NganHangKh { get; set; }
        public virtual DbSet<NganHangNcc> NganHangNcc { get; set; }
        public virtual DbSet<NhaCungCap> NhaCungCap { get; set; }
        public virtual DbSet<NhanVien> NhanVien { get; set; }
        public virtual DbSet<NhomHangHoa> NhomHangHoa { get; set; }
        public virtual DbSet<NhomNhanVien> NhomNhanVien { get; set; }
        public virtual DbSet<Nsx> Nsx { get; set; }
        public virtual DbSet<PhieuNhap> PhieuNhap { get; set; }
        public virtual DbSet<PhieuTraNo> PhieuTraNo { get; set; }
        public virtual DbSet<PhieuXuat> PhieuXuat { get; set; }
        public virtual DbSet<TaiKhoan> TaiKhoan { get; set; }
        public virtual DbSet<TrangThai> TrangThai { get; set; }
        public virtual DbSet<VaiTro> VaiTro { get; set; }
        public virtual DbSet<VaiTroTk> VaiTroTk { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=LAPTOP-J4VFA095;Initial Catalog=MedicalShop;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChiNhanh>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Address).HasMaxLength(250);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MaCn)
                    .HasColumnName("MaCN")
                    .HasMaxLength(50);

                entity.Property(e => e.Mail).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Note).HasMaxLength(500);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.TenCn)
                    .HasColumnName("TenCN")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<ChiTietPhieuNhap>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Hsd)
                    .HasColumnName("HSD")
                    .HasColumnType("datetime");

                entity.Property(e => e.Idbh).HasColumnName("IDBH");

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.Idhh).HasColumnName("IDHH");

                entity.Property(e => e.Idpn).HasColumnName("IDPN");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Nsx)
                    .HasColumnName("NSX")
                    .HasColumnType("datetime");

                entity.Property(e => e.Price)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Quantity).HasDefaultValueSql("((0))");

                entity.Property(e => e.SoLo).HasMaxLength(50);

                entity.Property(e => e.Tgbh).HasColumnName("TGBH");

                entity.HasOne(d => d.IdbhNavigation)
                    .WithMany(p => p.ChiTietPhieuNhap)
                    .HasForeignKey(d => d.Idbh)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ChiTietPhieuNhap_DVBH");

                entity.HasOne(d => d.IdhhNavigation)
                    .WithMany(p => p.ChiTietPhieuNhap)
                    .HasForeignKey(d => d.Idhh)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ChiTietPhieuNhap_HangHoa");

                entity.HasOne(d => d.IdpnNavigation)
                    .WithMany(p => p.ChiTietPhieuNhap)
                    .HasForeignKey(d => d.Idpn)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ChiTietPhieuNhap_PhieuNhap");
            });

            modelBuilder.Entity<ChiTietPhieuXuat>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Hsd)
                    .HasColumnName("HSD")
                    .HasColumnType("datetime");

                entity.Property(e => e.Iddvt).HasColumnName("IDDVT");

                entity.Property(e => e.Idhh).HasColumnName("IDHH");

                entity.Property(e => e.Idpn).HasColumnName("IDPN");

                entity.Property(e => e.Idpx).HasColumnName("IDPX");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Nsx)
                    .HasColumnName("NSX")
                    .HasColumnType("datetime");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18, 0)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Quantity).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.IdhhNavigation)
                    .WithMany(p => p.ChiTietPhieuXuat)
                    .HasForeignKey(d => d.Idhh)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ChiTietPhieuXuat_HangHoa");

                entity.HasOne(d => d.IdpxNavigation)
                    .WithMany(p => p.ChiTietPhieuXuat)
                    .HasForeignKey(d => d.Idpx)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ChiTietPhieuXuat_PhieuXuat");
            });

            modelBuilder.Entity<ChiTietTraNo>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Idpn).HasColumnName("IDPN");

                entity.Property(e => e.Idptn).HasColumnName("IDPTN");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Money).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.IdpnNavigation)
                    .WithMany(p => p.ChiTietTraNo)
                    .HasForeignKey(d => d.Idpn)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ChiTietTraNo_PhieuNhap");

                entity.HasOne(d => d.IdptnNavigation)
                    .WithMany(p => p.ChiTietTraNo)
                    .HasForeignKey(d => d.Idptn)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ChiTietTraNo_PhieuTraNo");
            });

            modelBuilder.Entity<ChucNang>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MaCnang)
                    .HasColumnName("MaCNang")
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.TenCnang)
                    .HasColumnName("TenCNang")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<Dvbh>(entity =>
            {
                entity.ToTable("DVBH");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Dvqd)
                    .HasColumnName("DVQD")
                    .HasMaxLength(50);

                entity.Property(e => e.LoaiTg)
                    .HasColumnName("LoaiTG")
                    .HasMaxLength(500);

                entity.Property(e => e.MaTg)
                    .HasColumnName("MaTG")
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Slqd).HasColumnName("SLQD");
            });

            modelBuilder.Entity<Dvt>(entity =>
            {
                entity.ToTable("DVT");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MaDvt)
                    .HasColumnName("MaDVT")
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.TenDvt)
                    .HasColumnName("TenDVT")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<Dvvc>(entity =>
            {
                entity.ToTable("DVVC");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.MaDvvc)
                    .HasColumnName("MaDVVC")
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Note).HasMaxLength(500);

                entity.Property(e => e.TenDvvc)
                    .HasColumnName("TenDVVC")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<HangHoa>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.BarCode).HasMaxLength(100);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Detail).HasColumnType("ntext");

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.Idhsx).HasColumnName("IDHSX");

                entity.Property(e => e.Idnhh).HasColumnName("IDNHH");

                entity.Property(e => e.Idnsx).HasColumnName("IDNSX");

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.MaHh)
                    .HasColumnName("MaHH")
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MoreImages).HasColumnType("xml");

                entity.Property(e => e.Quantity).HasDefaultValueSql("((0))");

                entity.Property(e => e.TenHh)
                    .HasColumnName("TenHH")
                    .HasMaxLength(250);

                entity.HasOne(d => d.IdhsxNavigation)
                    .WithMany(p => p.HangHoa)
                    .HasForeignKey(d => d.Idhsx)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_HangHoa_HSX");

                entity.HasOne(d => d.IdnhhNavigation)
                    .WithMany(p => p.HangHoa)
                    .HasForeignKey(d => d.Idnhh)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_HangHoa_NhomHangHoa");

                entity.HasOne(d => d.IdnsxNavigation)
                    .WithMany(p => p.HangHoa)
                    .HasForeignKey(d => d.Idnsx)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_HangHoa_NSX");
            });

            modelBuilder.Entity<HhDvt>(entity =>
            {
                entity.ToTable("HH_DVT");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Iddvt).HasColumnName("IDDVT");

                entity.Property(e => e.Idhh).HasColumnName("IDHH");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.IddvtNavigation)
                    .WithMany(p => p.HhDvt)
                    .HasForeignKey(d => d.Iddvt)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_HH_DVT_DVT");

                entity.HasOne(d => d.IdhhNavigation)
                    .WithMany(p => p.HhDvt)
                    .HasForeignKey(d => d.Idhh)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_HH_DVT_HangHoa");
            });

            modelBuilder.Entity<Hsx>(entity =>
            {
                entity.ToTable("HSX");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(250);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.MaHsx)
                    .HasColumnName("MaHSX")
                    .HasMaxLength(50);

                entity.Property(e => e.Mail).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.TenHsx)
                    .HasColumnName("TenHSX")
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<Httt>(entity =>
            {
                entity.ToTable("HTTT");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.MaHttt)
                    .HasColumnName("MaHTTT")
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Note).HasMaxLength(500);

                entity.Property(e => e.TenHttt)
                    .HasColumnName("TenHTTT")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<KhachHang>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Address).HasMaxLength(250);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.Idnv).HasColumnName("IDNV");

                entity.Property(e => e.MaKh)
                    .HasColumnName("MaKH")
                    .HasMaxLength(50);

                entity.Property(e => e.Mail).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Note).HasMaxLength(500);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.TenKh)
                    .HasColumnName("TenKH")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<NganHang>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.Idhttt).HasColumnName("IDHTTT");

                entity.Property(e => e.MaNh)
                    .HasColumnName("MaNH")
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Note).HasMaxLength(500);

                entity.Property(e => e.TenNh)
                    .HasColumnName("TenNH")
                    .HasMaxLength(500);

                entity.HasOne(d => d.IdhtttNavigation)
                    .WithMany(p => p.NganHang)
                    .HasForeignKey(d => d.Idhttt)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_NganHang_HTTT");
            });

            modelBuilder.Entity<NganHangKh>(entity =>
            {
                entity.ToTable("NganHang_KH");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Idkh).HasColumnName("IDKH");

                entity.Property(e => e.Idnh).HasColumnName("IDNH");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Stk)
                    .HasColumnName("STK")
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdkhNavigation)
                    .WithMany(p => p.NganHangKh)
                    .HasForeignKey(d => d.Idkh)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_NganHang_KH_KhachHang");

                entity.HasOne(d => d.IdnhNavigation)
                    .WithMany(p => p.NganHangKh)
                    .HasForeignKey(d => d.Idnh)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_NganHang_KH_NganHang");
            });

            modelBuilder.Entity<NganHangNcc>(entity =>
            {
                entity.ToTable("NganHang_NCC");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Idncc).HasColumnName("IDNCC");

                entity.Property(e => e.Idnh).HasColumnName("IDNH");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Stk)
                    .HasColumnName("STK")
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdnccNavigation)
                    .WithMany(p => p.NganHangNcc)
                    .HasForeignKey(d => d.Idncc)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_NganHang_NCC_NhaCungCap");

                entity.HasOne(d => d.IdnhNavigation)
                    .WithMany(p => p.NganHangNcc)
                    .HasForeignKey(d => d.Idnh)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_NganHang_NCC_NganHang");
            });

            modelBuilder.Entity<NhaCungCap>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Address).HasMaxLength(250);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.MaNcc)
                    .HasColumnName("MaNCC")
                    .HasMaxLength(50);

                entity.Property(e => e.Mail).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Note).HasMaxLength(500);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.TenNcc)
                    .HasColumnName("TenNCC")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<NhanVien>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Address).HasMaxLength(250);

                entity.Property(e => e.Cccd)
                    .HasColumnName("CCCD")
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.HomeTown).HasMaxLength(500);

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.Idnnv).HasColumnName("IDNNV");

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.MaNv)
                    .HasColumnName("MaNV")
                    .HasMaxLength(50);

                entity.Property(e => e.Mail).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MoreImages).HasColumnType("xml");

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.TenNv)
                    .HasColumnName("TenNV")
                    .HasMaxLength(500);

                entity.HasOne(d => d.IdnnvNavigation)
                    .WithMany(p => p.NhanVien)
                    .HasForeignKey(d => d.Idnnv)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_NhanVien_NhomNhanVien");
            });

            modelBuilder.Entity<NhomHangHoa>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.MaNhh)
                    .HasColumnName("MaNHH")
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.TenNhh)
                    .HasColumnName("TenNHH")
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<NhomNhanVien>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MaNnv)
                    .HasColumnName("MaNNV")
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.TenNnv)
                    .HasColumnName("TenNNV")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<Nsx>(entity =>
            {
                entity.ToTable("NSX");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.MaNsx)
                    .HasColumnName("MaNSX")
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.TenNsx)
                    .HasColumnName("TenNSX")
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<PhieuNhap>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.Idncc).HasColumnName("IDNCC");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.SoPn)
                    .HasColumnName("SoPN")
                    .HasMaxLength(50);

                entity.Property(e => e.TenPn)
                    .HasColumnName("TenPN")
                    .HasMaxLength(250);

                entity.HasOne(d => d.IdnccNavigation)
                    .WithMany(p => p.PhieuNhap)
                    .HasForeignKey(d => d.Idncc)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_PhieuNhap_NhaCungCap");
            });

            modelBuilder.Entity<PhieuTraNo>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateOfPayment).HasColumnType("datetime");

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.Idhttt).HasColumnName("IDHTTT");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.SoPhieu).HasMaxLength(50);

                entity.Property(e => e.TotalMoney).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.IdhtttNavigation)
                    .WithMany(p => p.PhieuTraNo)
                    .HasForeignKey(d => d.Idhttt)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_PhieuTraNo_HTTT");
            });

            modelBuilder.Entity<PhieuXuat>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.SoPx)
                    .HasColumnName("SoPX")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TaiKhoan>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");
              
                entity.Property(e => e.Roles).HasDefaultValueSql("((0))");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Idkh).HasColumnName("IDKH");

                entity.Property(e => e.Idnv).HasColumnName("IDNV");

                entity.Property(e => e.Idvt).HasColumnName("IDVT");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.UserName).HasMaxLength(50);

                entity.HasOne(d => d.IdkhNavigation)
                    .WithMany(p => p.TaiKhoan)
                    .HasForeignKey(d => d.Idkh)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_TaiKhoan_KhachHang");

                entity.HasOne(d => d.IdnvNavigation)
                    .WithMany(p => p.TaiKhoan)
                    .HasForeignKey(d => d.Idnv)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_TaiKhoan_NhanVien");
            });

            modelBuilder.Entity<TrangThai>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateOfPayment).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.Iddvvc).HasColumnName("IDDVVC");

                entity.Property(e => e.Idpx).HasColumnName("IDPX");

                entity.Property(e => e.MaTt)
                    .HasColumnName("MaTT")
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.NameOfShipper).HasMaxLength(500);

                entity.Property(e => e.Nvtt).HasColumnName("NVTT");

                entity.Property(e => e.Nvxk).HasColumnName("NVXK");

                entity.Property(e => e.PhoneOfShipper).HasMaxLength(50);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.TenTt)
                    .HasColumnName("TenTT")
                    .HasMaxLength(500);

                entity.HasOne(d => d.IddvvcNavigation)
                    .WithMany(p => p.TrangThai)
                    .HasForeignKey(d => d.Iddvvc)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_TrangThai_DVVC");

                entity.HasOne(d => d.IdpxNavigation)
                    .WithMany(p => p.TrangThai)
                    .HasForeignKey(d => d.Idpx)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_TrangThai_PhieuXuat");
            });

            modelBuilder.Entity<VaiTro>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.Idcnang).HasColumnName("IDCNang");

                entity.Property(e => e.MaVt)
                    .HasColumnName("MaVT")
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.TenVt)
                    .HasColumnName("TenVT")
                    .HasMaxLength(500);

                entity.HasOne(d => d.IdcnNavigation)
                    .WithMany(p => p.VaiTro)
                    .HasForeignKey(d => d.Idcn)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_VaiTro_ChiNhanh");

                entity.HasOne(d => d.IdcnangNavigation)
                    .WithMany(p => p.VaiTro)
                    .HasForeignKey(d => d.Idcnang)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_VaiTro_ChucNang");
            });

            modelBuilder.Entity<VaiTroTk>(entity =>
            {
                entity.ToTable("VaiTro_TK");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Idtk).HasColumnName("IDTK");

                entity.Property(e => e.Idvt).HasColumnName("IDVT");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.IdtkNavigation)
                    .WithMany(p => p.VaiTroTk)
                    .HasForeignKey(d => d.Idtk)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_VaiTro_TK_TaiKhoan");

                entity.HasOne(d => d.IdvtNavigation)
                    .WithMany(p => p.VaiTroTk)
                    .HasForeignKey(d => d.Idvt)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_VaiTro_TK_VaiTro");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
