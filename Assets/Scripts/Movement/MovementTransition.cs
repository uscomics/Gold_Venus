namespace USComics_Movement {
    [System.Serializable]
    class MovementModulesTransition  {
        public AbstractMovementModule From { get; set; }
        public AbstractMovementModule To { get; set; }

        public MovementModulesTransition(AbstractMovementModule from, AbstractMovementModule to) {
            From = from;
            To = to;
        }
    }
}
