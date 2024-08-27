using System;
using System.Collections.Generic;
using System.Threading;

class ProductorConsumidor
{
    private Queue<int> buffer = new Queue<int>();
    private int capacidad = 5;
    private object lockObject = new object();

    public void Productor()
    {
        int item = 0;
        while (true)
        {
            lock (lockObject)
            {
                while (buffer.Count == capacidad)
                {
                    // Esperar hasta que haya espacio en el buffer
                    Monitor.Wait(lockObject);
                }

                // Añadir el ítem al buffer
                buffer.Enqueue(item);
                Console.WriteLine($"Producto creado: {item}");

                // Notificar al consumidor que hay un ítem disponible
                Monitor.Pulse(lockObject);
            }

            item++;
            Thread.Sleep(1000); // Simula el tiempo de producción
        }
    }

    public void Consumidor()
    {
        while (true)
        {
            int item;

            lock (lockObject)
            {
                while (buffer.Count == 0)
                {
                    // Esperar hasta que haya un ítem en el buffer
                    Monitor.Wait(lockObject);
                }

                // Tomar un ítem del buffer
                item = buffer.Dequeue();
                Console.WriteLine($"Producto consumido: {item}");

                // Notificar al productor que hay espacio disponible
                Monitor.Pulse(lockObject);
            }

            Thread.Sleep(1500); // Simula el tiempo de consumo
        }
    }

    static void Main()
    {
        ProductorConsumidor pc = new ProductorConsumidor();

        Thread t1 = new Thread(new ThreadStart(pc.Productor));
        Thread t2 = new Thread(new ThreadStart(pc.Consumidor));

        t1.Start();
        t2.Start();

        t1.Join();
        t2.Join();
    }
}
