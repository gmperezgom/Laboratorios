using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Proyecto2_ParteB_Gerber_Pérez_1165825
{
    public class Program
    {
        static void Main(string[] args)
        {
            string opcionVolveraJugar;

            BatallaBarcos();
            while (true)
            {
                Console.WriteLine("¿Quieres jugar otra vez? (Si/No)");
                opcionVolveraJugar = Console.ReadLine().ToLower().Trim();

                if (opcionVolveraJugar == "si")
                {
                    Console.Clear();
                    BatallaBarcos();
                    break;
                }
                else if (opcionVolveraJugar == "no")
                {
                    Console.Clear();
                    Console.WriteLine("Gracias por jugar a este juego");
                    Console.WriteLine("GAMEEEEE OVERRR");
                    Console.ReadKey();
                    break;
                }
                else
                {
                    Console.WriteLine("Es una opción inválida. Por favor, escribe Si/No ");
                }
            }

        }
        static void BatallaBarcos() {
            //Ingreso del nickname1 por parte del usuario
            Console.Write("Ingresa el nickname del Jugador 1: ");
            string nick1 = Console.ReadLine();

            //Ingreso del nickname2 por parte del usuario
            Console.Write("Ingresa el nickname del Jugador 2: ");
            string nick2 = Console.ReadLine();

            //Asignacion del valor del nickname para cada uno de los jugadores
            Jugador jugador1 = new Jugador(nick1);
            Jugador jugador2 = new Jugador(nick2);

            //Declaracion de las variables
            var rnd = new Random();
            const int maximoTurnos = 15;
            int turnos1 = 0, turnos2 = 0, turnoActual = 0, puntos = 0;
            //Inicializacion de las variables para el objeto
            Jugador player1 = jugador1, player2 = jugador2, ganador = null;
            string orden;

            //Creacion de un arreglo temporal para repetir el proceso para ambos jugadores
            foreach (var jugador in new[] { jugador1, jugador2 })
            {
                do
                {
                    //Generar de manera aleatoria la flota por medio de un random declarado con anterioridad
                    jugador.GenerarFlotaAleatoria(rnd);
                    Console.Clear();
                    Console.WriteLine($"Posicion de la flota");
                    //Dibujar el tablero 
                    jugador.DibujarTablero(0);
                    do
                    {
                        //El usuario ingresa de las 2 maneras ya sea con mayusculas o minusculas
                        Console.Write("¿Aceptas dicha posición para tu flota naval? (Si/No): ");
                        orden = Console.ReadLine().Trim().ToLowerInvariant();
                    }
                    while (orden != "si" && orden != "no");
                }
                while (orden != "si");
                Console.Clear();
            }
            while (true)
            {
                //Si el jugador alcanza el máximo de turnos
                if ((player1 == jugador1 && turnos1 >= maximoTurnos) || (player1 == jugador2 && turnos2 >= maximoTurnos))
                {
                    break;
                }
                //Si el jugador 1 es igual a player 1
                if (player1 == jugador1)
                {
                    //El contador de turnos1 acumula de 1 en 1 
                    turnoActual = turnos1 + 1;
                }
                else
                {
                    //Caso contrario turnos2 acumula de 1 en 1
                    turnoActual = turnos2 + 1;
                }
                Console.Clear();
                //Dibuja el tablero en base al turno actual
                player1.DibujarTablero(turnoActual);
                //Se ingresa 1 o 2 dependiendo la opcion que desee el usuario
                Console.WriteLine("1) Lanzar un ataque   2) Rendirse");
                Console.Write("Opción: ");
                //Se realiza la valdacion de la opcion
                if (!int.TryParse(Console.ReadLine(), out int opcion))
                {
                    opcion = 0;
                }
                if (opcion == 1)
                {
                    //Se lanza el misil en base a las coordenas del oponente
                    var coordendadas = player1.LanzarMisil();
                    //Se inicializa una variable booleana para obtener si hubo acierto o no
                    bool acierto = player2.RecibirMisil(coordendadas);
                    //Se apunta el resultado segun las coordenadas y el acierto (O si acertó y X si no fue efectivo)
                    player1.ApuntarResultado(coordendadas, acierto);
                    if (acierto)
                    {
                        //Se realiza la verificacion del barco hundido para obtener los puntos
                        puntos = player2.VerificarBarcoHundido(coordendadas);
                        if (puntos > 0)
                        {
                            //Acumula el puntaje si fue efectiva la validación de la verificación
                            player1.Puntaje += puntos;
                        }

                    }
                    //Aumento de los turnos
                    if (player1 == jugador1)
                    {
                        turnos1++;
                    }
                    else
                    {
                        turnos2++;
                    }
                }
                //Si se ingresa 2 el ganador es otro jugador
                else if (opcion == 2)
                {
                    ganador = player2;
                    break;
                }
                //Caso contrario vuelve a pedir una opcion valida
                else
                {
                    Console.WriteLine("Selecciona una opción válida.");
                    Console.ReadKey();
                    continue;
                }
                if (player2.VerificarVictoria())
                {
                    ganador = player1;
                    break;
                }
                //Se crea una variable para invertir los roles en el ciclo
                var temp = player1;
                player1 = player2;
                player2 = temp;
            }
            Console.Clear();
            if (ganador == null)
            {
                //Si el numero de turnos es el maximo y el puntaje es igual, se declara como empate
                if (turnos1 == maximoTurnos && turnos2 == maximoTurnos && jugador1.Puntaje == jugador2.Puntaje)
                {
                    Console.WriteLine("¡Has empatado por el límite de turnos!");
                }
                else
                {
                    //Si el jugador 1 tiene mas puntos que el jugador 2
                    if (jugador1.Puntaje > jugador2.Puntaje)
                    {
                        Console.WriteLine($"El ganador es: {jugador1.Nombre}");
                    }
                    //Caso contrario el ganador es el jugador 2
                    else
                    {
                        Console.WriteLine($"El ganador es: {jugador2.Nombre}");
                    }
                }
            }
            else
            {
                Console.WriteLine($"El ganador de esta batalla es: {ganador.Nombre}");
            }
            //Se hace un resumen con el puntaje de cada uno de los jugadores
            Console.WriteLine($"Puntajes de cada jugador:  Jugador 1: {jugador1.Puntaje}  Jugador 2: {jugador2.Puntaje}");
            Console.WriteLine("Ha finalizado el juego, muchas gracias por jugar");
        }
    }
}