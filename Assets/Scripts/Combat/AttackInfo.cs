namespace USComics_Combat {
    [System.Serializable]
    public class AttackInfo {
        public Damage Damage;
        public float Range;
        public float Recharge;
        public float LastUsed;
        public int SuperBarValue;

        public AttackInfo() { }
        public AttackInfo(AttackInfo from) {
            Damage = new Damage(from.Damage);
            Range = from.Range;
            Recharge = from.Recharge;
            LastUsed = from.LastUsed;
            SuperBarValue = from.SuperBarValue;
        }
   }
}
