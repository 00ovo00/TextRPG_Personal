using System.Numerics;

namespace TextRPG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();

            // 게임 루프 - 매 50밀리초마다 게임을 처리
            while (true)
            {
                Thread.Sleep(50); // 짧은 대기 시간 추가
                game.Process();   // 게임 상태를 처리
            }
        }
    }
}