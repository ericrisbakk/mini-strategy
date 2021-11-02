namespace Source.StrategyFramework.Runtime {

    /// <summary>
    /// Ensure each strategy entity has a unique ID. Not a "proper" entity as in ECS, but it is convenient to call it
    /// so in this context.
    /// </summary>
    public class StrategyEntity {

        public static int NextID = 0;

        public static int GetNextID() {
            NextID += 1;
            return NextID;
        }
        
        public int ID;

        public StrategyEntity() {
            ID = GetNextID();
        }
    }
}
