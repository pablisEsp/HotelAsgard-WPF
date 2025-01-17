public class UsuarioSingleton
{
    private static UsuarioSingleton? instancia;

    public Usuario UsuarioActual { get; private set; }

    private UsuarioSingleton(Usuario usuario)
    {
        UsuarioActual = usuario;
    }

    public static UsuarioSingleton ObtenerInstancia(Usuario usuario)
    {
        if (instancia == null)
        {
            instancia = new UsuarioSingleton(usuario);
        }
        return instancia;
    }

    public static void CerrarSesion()
    {
        instancia = null;
    }
}