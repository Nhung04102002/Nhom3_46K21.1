﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace QLCH_Nhom2.Pages.KhachHang
{
    public class EditModel : PageModel
    {
        public KHInfo khInfo = new KHInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            string MaKH = Request.Query["MaKH"];

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "select * from KHACHHANG where MaKH=@MaKH";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@MaKH", MaKH);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                khInfo.MaKH = reader.GetString(0);
                                khInfo.TenKH = reader.GetString(1);
                                khInfo.SDT = reader.GetString(2);
                                khInfo.DiaChi = reader.GetString(3);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost()
        {
            khInfo.MaKH = Request.Form["MaKH"];
            khInfo.TenKH = Request.Form["TenKH"];
            khInfo.SDT = Request.Form["SDT"];
            khInfo.DiaChi = Request.Form["DiaChi"];

            if (khInfo.MaKH.Length == 0 || khInfo.SDT.Length == 0 || khInfo.TenKH.Length == 0)
            {
                errorMessage = "Tên và số điện thoại không được để trống";
                return;
            }

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "update KHACHHANG " + "set TenKH=@TenKH, DiaChi=@DiaChi, SDT=@SDT where MaKH=@MaKH";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@MaKH", khInfo.MaKH);
                        command.Parameters.AddWithValue("@TenKH", khInfo.TenKH);
                        command.Parameters.AddWithValue("@SDT", khInfo.SDT);
                        command.Parameters.AddWithValue("@DiaChi", khInfo.DiaChi);

                        command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/KhachHang/KhachHang");
        }


    }
}