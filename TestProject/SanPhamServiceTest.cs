using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test;

namespace TestProject
{
    [TestFixture]
    public class SanPhamServiceTest
    {
        private SanPhamService _service;

        [SetUp]
        public void Setup()
        {
            _service = new SanPhamService(new List<SanPham>
        {
            new(){ Ma="SP1", Ten="A", SoLuong=10, Gia=100 },
            new(){ Ma="SP2", Ten="B", SoLuong=5, Gia=200 }
        });
        }

        [Test] public void Xoa_MaTonTai_True() => Assert.IsTrue(_service.XoaSanPham("SP1"));
        [Test] public void Xoa_MaKhongTonTai_False() => Assert.IsFalse(_service.XoaSanPham("SP100"));
        [Test] public void Xoa_MaRong_False() => Assert.IsFalse(_service.XoaSanPham(""));
        [Test] public void Xoa_MaNull_False() => Assert.IsFalse(_service.XoaSanPham(null));
        [Test]
        public void Xoa_XoaXong_KhongConTrongDanhSach()
        {
            _service.XoaSanPham("SP1");
            Assert.IsNull(_service.GetAll().Find(s => s.Ma == "SP1"));
        }
    }
}
