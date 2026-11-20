// Boton "Calcular cuota" de los formularios de inscripcion y edicion.
// Es solo un calculo rapido para el usuario, el valor real lo calcula C# en el controlador.
let botonCuota = document.getElementById("calcular-cuota");

if (botonCuota) {
    botonCuota.addEventListener("click", function () {
        let listaPlanes = document.getElementById("PlanId");
        let opcion = listaPlanes.options[listaPlanes.selectedIndex];
        let cilindraje = Number(document.getElementById("Cilindraje").value);
        let info = document.getElementById("info-cuota");

        if (!opcion || !opcion.value) {
            info.innerHTML = "Primero selecciona un plan.";
            return;
        }

        let cuota = Number(opcion.dataset.precio);
        let nombrePlan = opcion.text.split("—")[0].trim();
        let texto = "Plan <strong>" + nombrePlan + "</strong>";

        if (cilindraje > 600) {
            cuota = cuota + 10000;
            texto += " + recargo por alto cilindraje";
        }

        info.innerHTML = texto + ": <strong>$" + cuota.toLocaleString("es-CO") + "</strong> al mes.";
    });
}
