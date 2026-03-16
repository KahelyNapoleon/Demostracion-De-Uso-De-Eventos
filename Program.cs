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

    public static void TurnoChangedEventMethod(object? sender, ChangeDateEventArgs e)
    {
        Console.WriteLine($"Cambio de fecha de {e.FechaAnterior} a {e.FechaNueva}");
    }

    public static  void EstadoChangedEventSuscriptor(object? sender, ChangeEstadoEventArgs e)
    {
        Console.WriteLine($"Hubo un cambio de fecha por ende el estado del turno es {e.EstadoNuevo} y el anterior {e.EstadoAnterior}");
    }

}
public enum Estados
{
    Disponible = 0, Reservado = 1, Confirmado = 2, Procesando = 3, Finalizado = 4, Modificado = 5
}

public class Turno
{
    public Estados estadoturno;
    public DateTime fechaHoraturno;

    public Turno(DateTime fechaHoraTurno, Estados estadoTurno)
    {
        this.estadoturno = estadoTurno;
        this.fechaHoraturno = fechaHoraTurno;
    }

    public event EventHandler<ChangeDateEventArgs>? TurnoChanged;
    public event EventHandler<ChangeEstadoEventArgs>? EstadoChanged;

    public void ChangeTurno(ChangeDateEventArgs e)
    {
        TurnoChanged?.Invoke(this, e);
    }
    public void ChangeEstado(ChangeEstadoEventArgs e)
    {
        EstadoChanged?.Invoke(this, e);
    }

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
