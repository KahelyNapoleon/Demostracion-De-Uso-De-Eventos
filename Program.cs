using System;

namespace Evento3;

public class Program
{
    public static void Main()
    {
        Turno turnoElicia= new Turno(DateTime.Now, Estados.Confirmado);

        turnoElicia.TurnoChanged += TurnoChangedEventMethod;
        turnoElicia.EstadoChanged += EstadoChangedEventSuscriptor;

        turnoElicia.FechaHoraTurno = new DateTime(2026, 3, 25);

    }

    //Metodos para suscribir al evento
    public static void TurnoChangedEventMethod(object? sender, ChangeDateEventArgs e)
    {
        Console.WriteLine($"Cambio de fecha de {e.FechaAnterior} a {e.FechaNueva}");
    }

    public static  void EstadoChangedEventSuscriptor(object? sender, ChangeEstadoEventArgs e)
    {
        Console.WriteLine($"Hubo un cambio de fecha por ende el estado del turno es {e.EstadoNuevo} y el anterior {e.EstadoAnterior}");
    }

}

//Valores de los Estados de un Turno
public enum Estados
{
    Disponible = 0, Reservado = 1, Confirmado = 2, Procesando = 3, Finalizado = 4, Modificado = 5
}


//Clase Publicadora de los eventos
public class Turno
{
    //Campos de la Clase: Defienen el estado de la clase
    public Estados estadoturno;
    public DateTime fechaHoraturno;

    public Turno(DateTime fechaHoraTurno, Estados estadoTurno)
    {
        this.estadoturno = estadoTurno;
        this.fechaHoraturno = fechaHoraTurno;
    }


    //Eventos de la clase
    public event EventHandler<ChangeDateEventArgs>? TurnoChanged;
    public event EventHandler<ChangeEstadoEventArgs>? EstadoChanged;

    //Metodos que lanzan los Eventos de la clase publicadora: Una vez tengan suscriptores
    public void ChangeTurno(ChangeDateEventArgs e)
    {
        TurnoChanged?.Invoke(this, e);
    }
    public void ChangeEstado(ChangeEstadoEventArgs e)
    {
        EstadoChanged?.Invoke(this, e);
    }

    //Propidades para acceder a los campos y leer o modificar sus valores:

    //Propiedad para el campo EstadoTurno:
    private Estados EstadoTurno
    {
        get { return this.estadoturno; }
        set
        {
            var estadoAnterior = this.estadoturno;
            this.estadoturno = value;

            var args = new ChangeEstadoEventArgs(estadoAnterior, this.estadoturno);
            ChangeEstado(args);
        }
    }

    //Propiedad para el campo FechaHoraTurno
    public DateTime FechaHoraTurno
    {
        get => this.fechaHoraturno;
        set
        {
            if (this.fechaHoraturno == value) return;

            var fechaAnterior = this.fechaHoraturno;
            this.fechaHoraturno = value;

            var args = new ChangeDateEventArgs(fechaAnterior,this.fechaHoraturno);
            ChangeTurno(args);

            //El hecho de realizar una asignacion a esta Propiedad 'EstadoTurno' enciende el metodo
            //de seteo, lanzando el evento de cambio de Estado 'ChangeEstado() => EstadoChanged(this, e)' donde 'e' contiene datos para transferir al evento -> 'ChangeEstadoTurnoArgs(estadoTurnoAnterior, estadoTurnoActual)'
            //este metodo Sirve para transferir datos a los eventos como argumentos.
            EstadoTurno = Estados.Modificado;

           

        }

    }


}

public class ChangeDateEventArgs : EventArgs
{
    public DateTime FechaAnterior { get; set; }
    public DateTime FechaNueva { get; set; }


    public ChangeDateEventArgs(DateTime fechaAnterior, DateTime fechaNueva)
    {
        FechaAnterior = fechaAnterior;
        FechaNueva = fechaNueva;
    }
}

public class ChangeEstadoEventArgs : EventArgs
{
    public Estados EstadoAnterior { get; set; }
    public Estados EstadoNuevo { get; set; }

    public ChangeEstadoEventArgs(Estados estadoAnterior, Estados estadoNuevo)
    {
        EstadoAnterior = estadoAnterior;
        EstadoNuevo = estadoNuevo;
    }
}
