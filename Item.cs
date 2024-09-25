using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    // 기본 아이템 클래스
    internal class Item
    {
        // 필드
        private string _name;
        private string _description;
        private int _price;
        private bool _isOwn;
        private bool _isSet;

        // 프로퍼티
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public bool IsOwn { get; set; }  // 구매 여부
        public bool IsSet { get; set; }  // 장착 여부

        // 아이템 생성자 - 기본 정보 설정
        protected Item(string name, string description, int price, bool isOwn = false, bool isSet = false)
        {
            Name = name;
            Description = description;
            Price = price;
            IsOwn = isOwn;
            IsSet = isSet;
        }
    }

    // 공격 아이템 클래스
    internal class AttackItem : Item
    {
        private float _attack;

        // 공격력 프로퍼티 - 0 이상일 때만 설정 가능
        public float Attack
        {
            get { return _attack; }
            set
            {
                if (value >= 0)
                    _attack = value;
            }
        }

        // 공격 아이템 생성자
        public AttackItem(string name, float attack, string description, int price, bool isOwn = false, bool isSet = false)
            : base(name, description, price, isOwn, isSet)
        {
            Attack = attack;
        }
    }

    // 방어 아이템 클래스
    internal class DefenseItem : Item
    {
        private float _defense;

        // 방어력 프로퍼티 - 0 이상일 때만 설정 가능
        public float Defense
        {
            get { return _defense; }
            set
            {
                if (value >= 0)
                    _defense = value;
            }
        }

        // 방어 아이템 생성자
        public DefenseItem(string name, float defense, string description, int price, bool isOwn = false, bool isSet = false)
            : base(name, description, price, isOwn, isSet)
        {
            Defense = defense;
        }
    }
}
