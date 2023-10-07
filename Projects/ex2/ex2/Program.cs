using System;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8; // set encoding UTF-8 cho Console
        int a, b;
        while (true)
        {
            try
            {
                Console.Write("Nhập số nguyên a: ");
                a = int.Parse(Console.ReadLine());
                Console.Write("Nhập số nguyên b: ");
                b = int.Parse(Console.ReadLine());
                break;
            }
            catch (FormatException)
            {
                Console.WriteLine("Định dạng không hợp lệ. Vui lòng nhập lại.");
            }
        }

        int sum = a + b;
        int difference = a - b;
        int product = a * b;

        Console.WriteLine("Tổng: " + sum);
        Console.WriteLine("Hiệu: " + difference);
        Console.WriteLine("Tích: " + product);

        try
        {
            int quotient_integer = a / b;
            float quotient_float = (float)a / b;

            Console.WriteLine("Thương (số nguyên): " + quotient_integer);
            Console.WriteLine("Thương (số thực): " + quotient_float);
        }
        catch (DivideByZeroException)
        {
            Console.WriteLine("Lỗi chia cho số 0. Không thể tính thương.");
        }
    }
}