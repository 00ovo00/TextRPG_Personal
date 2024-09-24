using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    class Utility
    {
        // 사용자 입력을 받고 메시지 타입에 따라 적절한 메시지를 출력하는 함수
        public int InputFromUser(ref MessageType msgType)
        {
            Console.WriteLine();
            switch (msgType)
            {
                case MessageType.WrongInput:
                    Console.WriteLine("잘못된 입력입니다.");
                    break;
                case MessageType.AlreadyBought:
                    Console.WriteLine("이미 구매한 아이템입니다.");
                    break;
                case MessageType.PurchaseSucceed:
                    Console.WriteLine("구매를 완료했습니다. ");
                    break;
                case MessageType.LackGold:
                    Console.WriteLine("Gold 가 부족합니다.");
                    break;
            }
            Console.WriteLine("\n원하시는 행동을 입력해주세요.");
            Console.Write(">> ");

            // 입력된 문자열을 숫자로 변환, 실패 시 -1 반환
            if (int.TryParse(Console.ReadLine(), out int result))
            {
                return result;
            }
            return -1;
        }

        // 주어진 텍스트의 너비(한글과 영어를 구분)를 계산하는 함수
        public int AlignWidth(string text)
        {
            int width = 0;
            foreach (char c in text)
            {
                // 한글일 경우 너비를 2로, 영어일 경우 1로 계산
                // if (c >= 'ㄱ' && c <= 'ㅎ' || c >= '가' && c <= '힣')
                if (c >= 0x1100 && c <= 0x11FF || c >= 0xAC00 && c <= 0xD7A3)
                {
                    width += 2;
                }
                else
                {
                    width += 1;
                }
            }
            return width;
        }

        // 주어진 너비에 맞춰 텍스트를 오른쪽으로 패딩하는 함수
        public string PadRightWithVisualWidth(string text, int totalWidth)
        {
            int visualWidth = AlignWidth(text);  // 텍스트의 너비 계산
            int paddingNeeded = totalWidth - visualWidth;  // 필요한 패딩 계산
            return text + new string(' ', Math.Max(paddingNeeded, 0));  // 패딩을 추가하여 반환
        }
    }
}
