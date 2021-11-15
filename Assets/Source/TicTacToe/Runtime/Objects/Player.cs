namespace Source.TicTacToe.Runtime.Objects {
    public class Player {
        // TODO: Fix how to identify different players.
        public bool IsPlayer0 { get; }

        public Player(bool isPlayer0) {
            IsPlayer0 = isPlayer0;
        }
    }
}