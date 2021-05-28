using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Games
{
    /**
    Меню с вариантами выбора
    */
    class SelectionMenu : IDrawableElement, IInteractive
    {
        public IDrawable[] ElementContent => getContent();
        /// Выбран ли како-нибудь вариант
        public bool IsSelected = false;
        public bool IsFocused { get => isFocused; set => isFocused = value; }
        /// Выбранный вариант
        public int SelectedIndex { get; private set; }

        bool isFocused = true;
        Border Border;
        TextField[] Variants => GetTextFields(str_variants, SelectedIndex);

        string[] str_variants;
        /// Отступ сверху и снизу (отступы одинаковы)
        int padding_topbuttom;
        /// Отступ слева и справа
        int padding_leftright;
        int field_width;
        int field_height;

        int default_selected;

        int menu_height;
        int menu_width;

        public SelectionMenu(string[] str_variants, int field_width, int field_height, int defaultSelected, Padding p)
        {
            this.SelectedIndex = defaultSelected;
            this.field_width = field_width;
            this.field_height = field_height;
            this.str_variants = str_variants;
            this.default_selected = defaultSelected;

            field_width -= (p.Left + p.Right);
            field_height -= (p.Buttom + p.Top);

            // Длина самого длинного слова
            int max_variant_length = str_variants.OrderByDescending(n => n.Length).First().Length + 2;

            this.menu_height = (str_variants.Length * 2) + 2;
            this.menu_width = max_variant_length + 4;

            this.padding_topbuttom = (field_height - menu_height) / 2;
            this.padding_leftright = (field_width - menu_width) / 2;

            /*
            Расчитать отступы таким образом чтобы меню было по середине
            */
            Padding p_border = new Padding(padding_leftright, padding_leftright,
                                padding_topbuttom, padding_topbuttom);

            this.Border = new Border('+', field_width, field_height, p_border);
        }

        /// Сбросить сделанный выбор, вернуть в начальное состояние
        public void Reuse()
        {
            this.SelectedIndex = default_selected;
            this.IsSelected = false;
        }

        public void HandleKey(ConsoleKey key)
        {
            if (key == ConsoleKey.W || key == ConsoleKey.K || key == ConsoleKey.UpArrow)
            {
                if (SelectedIndex == 0)
                {
                    SelectedIndex = str_variants.Length - 1;
                    return;
                }
                SelectedIndex -= 1;
            }
            else if (key == ConsoleKey.S || key == ConsoleKey.J || key == ConsoleKey.DownArrow)
            {
                if (SelectedIndex == str_variants.Length - 1)
                {
                    SelectedIndex = 0;
                    return;
                }
                SelectedIndex += 1;
            }
            else if (key == ConsoleKey.Enter || key == ConsoleKey.Spacebar)
            {
                IsSelected = true;
            }
        }

        private TextField[] GetTextFields(string[] variants, int selectedIndex)
        {
            TextField[] fields = new TextField[variants.Length];
            for (int i = 0; i < variants.Length; i++)
            {
                string text = variants[i];

                if (i == selectedIndex)
                {
                    text = $">{text}<";
                }
                else
                {
                    text = $" {text} "; // добавить пробелы чтобы затереть старые > <
                }

                Point textStartLocation = new Point((field_width / 2) - (text.Length / 2) - 1,
                                    padding_topbuttom + (i * 2) + 2);

                TextField text_field = new TextField(textStartLocation, text.Length, text);
                fields[i] = text_field;
            }

            return fields;
        }

        private IDrawable[] getContent()
        {
            List<DrawableChar> chars = new List<DrawableChar>();

            foreach (DrawableChar c in Border.ElementContent)
            {
                chars.Add(c);
            }

            foreach (TextField variant_field in Variants)
            {
                foreach (DrawableChar c in variant_field.ElementContent)
                {
                    chars.Add(c);
                }
            }

            return chars.ToArray();
        }
    }
}