using System;
using System.Drawing;
using System.Windows.Forms;

namespace Kursach.Forms
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();

            // Настройка comboBox1 (Language)
            comboBox1.Items.Add("English");
            comboBox1.Items.Add("Russian");
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList; // запрет ручного ввода
            comboBox1.SelectedIndex = 0; // значение по умолчанию

            comboBox1.DrawMode = DrawMode.OwnerDrawFixed;
            comboBox1.DrawItem += ComboBox_DrawItem;

            // Настройка comboBox2 (Theme)
            comboBox2.Items.Add("Light");
            comboBox2.Items.Add("Dark");
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList; // запрет ручного ввода
            comboBox2.SelectedIndex = 0; // значение по умолчанию

            comboBox2.DrawMode = DrawMode.OwnerDrawFixed;
            comboBox2.DrawItem += ComboBox_DrawItem;

            // Подписка на события выбора
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
        }

        // Общий обработчик отрисовки элементов ComboBox без синего фона и рамки
        private void ComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox combo = sender as ComboBox;

            if (e.Index < 0)
                return;

            string text = combo.Items[e.Index].ToString();

            // Рисуем белый фон всегда (без синего при наведении)
            e.Graphics.FillRectangle(Brushes.White, e.Bounds);

            // Рисуем текст черным цветом
            TextRenderer.DrawText(e.Graphics, text, combo.Font, e.Bounds, Color.Black, TextFormatFlags.Left);

            // Не рисуем пунктирную рамку (не вызываем e.DrawFocusRectangle())
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Здесь можно обработать выбор языка
            string selectedLanguage = comboBox1.SelectedItem.ToString();
            // Например, сохранить настройку или применить язык
            Console.WriteLine("Selected language: " + selectedLanguage);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Здесь можно обработать выбор темы
            string selectedTheme = comboBox2.SelectedItem.ToString();
            // Например, сохранить настройку или применить тему
            Console.WriteLine("Selected theme: " + selectedTheme);
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            // Если нужно, можно загрузить сохранённые настройки и установить SelectedIndex здесь
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Обработка клика по label1, если нужна
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // Обработка клика по label2, если нужна
        }
    }
}
