using Lab06_BUS;
using Lab06_DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace lab06_NguyenTranThienY
{
    public partial class Form1 : Form
    {
        private readonly StudentService studentService = new StudentService();
        private readonly FacultyService facultyService = new FacultyService();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            setGridViewStyle(dgvStudent);
            var listFacultys = facultyService.GetAll();
            var listStudents = studentService.GetAll();
            FillFacultyCombobox(listFacultys);
            BindGrid(listStudents);
        }
        private void FillFacultyCombobox(List<Faculty> listFacultys)
        {
            listFacultys.Insert(0, new Faculty());
            this.cmbFaculty.DataSource = listFacultys;
            this.cmbFaculty.DisplayMember = "FacultyName";
            this.cmbFaculty.ValueMember = "FacultyID";
        }
        private void BindGrid(List<Student> listStudent)
        {
            dgvStudent.Rows.Clear();
            foreach (var item in listStudent)
            {
                int index = dgvStudent.Rows.Add();
                dgvStudent.Rows[index].Cells[0].Value = item.StudentID;
                dgvStudent.Rows[index].Cells[1].Value = item.FullName;
                if (item.Faculty != null)
                    dgvStudent.Rows[index].Cells[2].Value = item.Faculty.FacultyName;
                dgvStudent.Rows[index].Cells[3].Value = item.AverageScore + "";
                if (item.MajorID != null)
                    dgvStudent.Rows[index].Cells[4].Value = item.Major.Name + "";
                ShowAvatar(item.Avatar);
            }
        }
        private void ShowAvatar(string ImageName)
        {
            if (string.IsNullOrEmpty(ImageName))
                picAvatar.Image = null;
            else
            {
                string parentDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                string imagePath = Path.Combine(parentDirectory, "Images", ImageName);
                picAvatar.Image = Image.FromFile(imagePath);
                picAvatar.Refresh();
            }
        }
        public void setGridViewStyle(DataGridView dgview)
        {
            dgview.BorderStyle = BorderStyle.None;
            dgview.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dgview.CellBorderStyle =
            DataGridViewCellBorderStyle.SingleHorizontal;
            dgview.BackgroundColor = Color.White;
            dgview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            var listStudents = new List<Student>();
            if (this.checkBox1.Checked)
                listStudents = studentService.GetAllHasNoMajor();
            else
                listStudents = studentService.GetAll();
            BindGrid(listStudents);
        }

        private void btnAddPicture_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                picAvatar.Image = Image.FromFile(dlg.FileName);
                string studentID = txtMSSV.Text;
                string filePath = dlg.FileName;

                // Đường dẫn tới thư mục "Images"
                string parentDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                string imagesDirectory = Path.Combine(parentDirectory, "Images");

                // Lấy phần mở rộng của file
                string extension = Path.GetExtension(filePath);
                string imageName = $"{studentID}{extension}"; // Tên file mới
                string destinationPath = Path.Combine(imagesDirectory, imageName);

                // Lưu hình ảnh vào thư mục
                File.Copy(filePath, destinationPath, true);
            }
        }
        private void dgvStudent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectrow = e.RowIndex;
            txtMSSV.Text = dgvStudent.Rows[selectrow].Cells["Column1"].Value.ToString();
            txtHoten.Text = dgvStudent.Rows[selectrow].Cells["Column2"].Value.ToString();
            cmbFaculty.Text = dgvStudent.Rows[selectrow].Cells["Column3"].Value.ToString();
            txtDTB.Text = dgvStudent.Rows[selectrow].Cells["Column4"].Value.ToString();


            string studentId = txtMSSV.Text; // Hoặc tên cột khác chứa định danh
            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" }; // Các định dạng hỗ trợ
            string imageName = studentId; // Bắt đầu với tên sinh viên

            // Kiểm tra các định dạng hình ảnh
            foreach (string extension in imageExtensions)
            {
                string imagePath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName, "Images", imageName + extension);
                if (File.Exists(imagePath))
                {
                    ShowAvatar(imageName + extension);
                    return; // Nếu tìm thấy, thoát khỏi vòng lặp
                }
            }

            // Nếu không tìm thấy hình ảnh, gọi ShowAvatar với null
            ShowAvatar(null);
        }
        private void btnThemSua_Click(object sender, EventArgs e)
        {
            try
            {
                string studentID = txtMSSV.Text.Trim(); 
                string fullName = txtHoten.Text.Trim();
                int facultyID = (int)cmbFaculty.SelectedValue;
                if (!double.TryParse(txtDTB.Text.Trim(), out var averageScore))
                {
                    MessageBox.Show("Vui lòng nhập điểm trung bình hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrEmpty(studentID) || string.IsNullOrEmpty(fullName))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var student = studentService.FindById(studentID);

                if (student == null)
                {
                    student = new Student { StudentID = studentID };
                    MessageBox.Show("Thêm mới sinh viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Cập nhật thông tin sinh viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                student.FullName = fullName;
                student.FacultyID = facultyID;
                student.AverageScore = averageScore;
                studentService.InsertUpdate(student);
                MessageBox.Show("Lưu thông tin sinh viên thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                BindGrid(studentService.GetAll());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvStudent.SelectedRows.Count > 0)
                {
                    var selectedRow = dgvStudent.SelectedRows[0];
                    string studentID = selectedRow.Cells[0].Value?.ToString();

                    if (!string.IsNullOrEmpty(studentID))
                    {
                        var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (confirmResult == DialogResult.Yes)
                        {
                            studentService.Delete(studentID);

                            MessageBox.Show("Xóa sinh viên thành công.");

                            var listStudents = studentService.GetAll();
                            BindGrid(listStudents);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không thể tìm thấy mã số sinh viên để xóa.");
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn một sinh viên trong danh sách để xóa.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi xóa sinh viên: " + ex.Message);
            }
        }
    }
}

