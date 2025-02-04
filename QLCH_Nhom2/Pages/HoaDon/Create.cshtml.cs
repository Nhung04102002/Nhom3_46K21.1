﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QLCH_Nhom2.Pages.Hang;
using QLCH_Nhom2.Pages.KhachHang;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QLCH_Nhom2.Pages.HoaDon
{
    public class CreateModel : PageModel
    {
        public HDInfo hd = new HDInfo();
        public List<KHInfo> listKH = new List<KHInfo>();
        public KHInfo searchInfo = new KHInfo();
        
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            searchInfo.Search = Request.Query["Search"];
            string MaKh = Request.Query["MaKH"];
            DateTime now = DateTime.Now;
            hd.NgayBan = now;
            hd.MaKH = MaKh;
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var search = new List<string>() { searchInfo.Search };
                    String sql3 = "select * from KHACHHANG where SDT like '%" + search[0] + "%' or MaKH like '%" + search[0] + "%'";
                    String sql = "select dbo.fNewMaHD()";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                hd.MaHD = reader.GetString(0);
                            }
                        }
                    }
                    using (SqlCommand command = new SqlCommand(sql3, connection))
                    {
                        command.Parameters.AddWithValue("@Search", searchInfo.Search);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                KHInfo KH = new KHInfo();
                                KH.MaKH = reader.GetString(0);
                                KH.TenKH = reader.GetString(1);
                                KH.SDT = reader.GetString(2);
                                KH.DiaChi = reader.GetString(3);

                                listKH.Add(KH);
                            }
                        }

                    }

                }
            }
            catch (Exception)
            {
            }
        }
        
        
        public void OnPost()
        {
            hd.MaHD = Request.Form["MaHD"];
            hd.MaKH = Request.Form["MaKH"];
            hd.NgayBan = Convert.ToDateTime(Request.Form["NgayBan"]);
            hd.ChietKhau = Convert.ToDouble(Request.Form["ChietKhau"]);

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    String sql2 = "insert into HOADON values (@MaHD, @MaKH, @NgayBan, 0, @ChietKhau, 0)";

                    using (SqlCommand command = new SqlCommand(sql2, connection))
                    {
                        command.Parameters.AddWithValue("@MaHD", hd.MaHD);
                        command.Parameters.AddWithValue("@MaKH", hd.MaKH);
                        command.Parameters.AddWithValue("@NgayBan", hd.NgayBan);
                        command.Parameters.AddWithValue("@ChietKhau", hd.ChietKhau);

                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            Response.Redirect("/HoaDon/CTHD");


        }
    }
}
