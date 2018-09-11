namespace USComics_Movement {
    [System.Serializable]
    class MovementTransition  {
        public AbstractMovementModule From { get; set; }
        public AbstractMovementModule To { get; set; }

        public MovementTransition(AbstractMovementModule from, AbstractMovementModule to) {
            From = from;
            To = to;
        }
    }
}
