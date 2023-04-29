using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace InterpolLibrary
{
    /// <summary>
    /// Список степеней защиты пароля.
    /// </summary>
    public enum PasswordStrength
    {
        NotSecure,
        Weak,
        Good,
        Strong
    }
    public class PasswordChecker
    {
        /// <summary>
        /// Оценка сложности введенного пароля.
        /// </summary>
        /// <param name="password">Введенный пароль.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Отсутствие возможности оценить пароль.</exception>
        public PasswordStrength CheckPassword(string password)
        {
            int score = 0;

            // Проверка длины пароля.
            if (password.Length >= 8)
            {
                score++;
            }

            // Поиск заглавных букв.
            if (Regex.IsMatch(password, "[A-ZА-Я]"))
            {
                score++;
            }

            // Поиск строчных букв.
            if (Regex.IsMatch(password, "[a-zа-я]"))
            {
                score++;
            }

            // Поиск цифр.
            if (Regex.IsMatch(password, @"\d"))
            {
                score++;
            }

            // Поиск символов.
            if (Regex.IsMatch(password, @"[^\w\s]"))
            {
                score++;
            }

            // Сравнение итогового количества очков защиты со степенью надежности.
            switch (score)
            {
                case 0:
                case 1:
                    return PasswordStrength.NotSecure;

                case 2:
                case 3:
                    return PasswordStrength.Weak;

                case 4:
                    return PasswordStrength.Good;

                case 5:
                    return PasswordStrength.Strong;

                default:
                    throw new System.Exception("Не удалось оценить сложность пароля");
            }
        }
    }
}
