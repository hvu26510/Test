using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class SanPhamService
    {
        private readonly List<SanPham> _ds;

        public SanPhamService(List<SanPham> initial) => _ds = initial ?? new();

        public bool XoaSanPham(string ma)
        {
            if (string.IsNullOrWhiteSpace(ma)) return false;
            var sp = _ds.FirstOrDefault(x => x.Ma == ma);
            if (sp == null) return false;
            _ds.Remove(sp);
            return true;
        }

        public List<SanPham> GetAll() => _ds;
    }
}
