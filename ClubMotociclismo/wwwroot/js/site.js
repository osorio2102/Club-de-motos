// Boton "Calcular cuota" del formulario de inscripcion.
// Es solo un calculo rapido para el usuario, el valor real lo calcula C# en el controlador.
let botonCuota = document.getElementById("calcular-cuota");

if (botonCuota) {
    botonCuota.addEventListener("click", function () {
        let planMarcado = document.querySelector('input[name="Plan"]:checked');
        let cilindraje = Number(document.getElementById("Cilindraje").value);
        let info = document.getElementById("info-cuota");

        if (!planMarcado) {
            info.innerHTML = "Primero selecciona un plan.";
            return;
        }

        let cuota = Number(planMarcado.dataset.precio);
        let texto = "Plan <strong>" + planMarcado.value + "</strong>";

        if (cilindraje > 600) {
            cuota = cuota + 10000;
            texto += " + recargo por alto cilindraje";
        }

        info.innerHTML = texto + ": <strong>$" + cuota.toLocaleString("es-CO") + "</strong> al mes.";
    });
}
