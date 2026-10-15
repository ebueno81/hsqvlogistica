namespace HsqvLogistica.States
{
    public class FiltroStateService
    {
        private readonly Dictionary<Type, object> _estados = new();

        public T Obtener<T>() where T : FiltroBaseState, new()
        {
            var tipo = typeof(T);

            if (!_estados.TryGetValue(tipo, out var estado))
            {
                estado = new T();
                _estados[tipo] = estado;
            }

            return (T)estado;
        }

        public void Limpiar<T>() where T : FiltroBaseState, new()
        {
            _estados.Remove(typeof(T));
        }

        public void LimpiarTodo()
        {
            _estados.Clear();
        }
    }
}
