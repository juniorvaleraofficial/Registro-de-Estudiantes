# Control Académico Estudiantil

Aplicación móvil académica desarrollada para la asignatura **INF-4316 - Programación de Aplicaciones Móviles**.

Este repositorio corresponde al proyecto móvil del **Grupo #6**. Su desarrollo se organiza mediante ramas de Git para conservar claramente los avances realizados en cada unidad.

---

## Integrantes

- **Junior Alexis Valera Rijo**  
  Matrícula: **MT-2023-00518**

- **Yisel del Carmen Santana Rosario**  
  Matrícula: **SD-18-11014**

---

## Rama actual

```text
unidad-6
```

La rama **unidad-6** corresponde a la continuación y mejora del proyecto académico mediante la incorporación de persistencia local con SQLite, operaciones asíncronas y nuevas funciones de consulta académica.

---

## Objetivo de la Unidad 6

Implementar una base de datos local con **SQLite** para almacenar permanentemente la información académica de la aplicación.

En las unidades anteriores, los datos permanecían temporalmente en memoria y podían perderse al cerrar la aplicación. En esta unidad, los registros se guardan en el dispositivo y continúan disponibles después de cerrar sesión, finalizar la aplicación o volver a abrirla.

También se adaptaron los servicios y ViewModels para trabajar con operaciones asíncronas, evitando bloquear la interfaz mientras se consulta o modifica la base de datos.

---

## Principales avances de la Unidad 6

- Integración de SQLite mediante el paquete `sqlite-net-pcl`.
- Creación automática de la base de datos local.
- Persistencia de estudiantes, materias, asistencias y calificaciones.
- Conversión de las operaciones del servicio académico a métodos asíncronos.
- CRUD persistente en los cuatro módulos académicos.
- Restricciones para evitar matrículas, códigos y registros duplicados.
- Datos académicos iniciales cargados sin duplicarse.
- Buscador y filtro de estudiantes en tiempo real.
- Resumen estadístico en la pantalla de inicio.
- Perfil académico individual de cada estudiante.
- Mejoras de desplazamiento e interacción en los formularios.
- Corrección visual de la pantalla de Asistencias en Android.
- Validación funcional completa en el emulador Android.

---

## Descripción de la aplicación

**Control Académico Estudiantil** es una aplicación móvil desarrollada con **.NET MAUI** para administrar información académica desde dispositivos Android y Windows.

La aplicación permite trabajar con los siguientes módulos:

- Estudiantes.
- Materias.
- Asistencias.
- Calificaciones.
- Inicio con resumen estadístico.
- Perfil académico del estudiante.

Los datos se almacenan localmente mediante SQLite y son administrados por un servicio académico asíncrono.

---

## Tecnologías utilizadas

- **C#** como lenguaje de programación.
- **.NET MAUI** para la interfaz móvil multiplataforma.
- **XAML** para la construcción de las pantallas.
- **MVVM** para separar la presentación de la lógica.
- **SQLite** para la persistencia local.
- **sqlite-net-pcl** para acceder a SQLite desde C#.
- **Git y GitHub** para el control de versiones.
- **Visual Studio 2022** como entorno principal de desarrollo.
- **Emulador Android** para las pruebas funcionales.

---

## Arquitectura y organización

El proyecto móvil separa sus responsabilidades en diferentes carpetas:

```text
RegistroEstudiantes.Mobile/
├── Models/
├── Services/
├── ViewModels/
├── Views/
├── Resources/
├── App.xaml
├── AppShell.xaml
└── MauiProgram.cs
```

### Models

Contiene las entidades académicas que representan las tablas de SQLite:

- `Estudiante`
- `Materia`
- `Asistencia`
- `Calificacion`

Los modelos utilizan identificadores enteros y atributos de SQLite para definir claves primarias, valores autoincrementales y restricciones únicas.

### Services

Contiene `ServicioAcademico`, responsable de:

- Inicializar la conexión con SQLite.
- Crear las tablas si todavía no existen.
- Insertar los datos iniciales.
- Consultar registros.
- Agregar nuevos registros.
- Actualizar registros existentes.
- Eliminar registros.
- Evitar que los datos de prueba se dupliquen.

### ViewModels

Contiene la lógica de presentación y las operaciones asíncronas de cada módulo:

- `StudentsViewModel`
- `MateriasViewModel`
- `AsistenciasViewModel`
- `CalificacionesViewModel`

Los ViewModels administran los formularios, las validaciones, el modo de edición, las listas observables, los contadores y los mensajes mostrados al usuario.

### Views

Contiene las páginas XAML y su código asociado:

- `LoginPage`
- `HomePage`
- `StudentsPage`
- `MateriasPage`
- `AsistenciasPage`
- `CalificacionesPage`
- `PerfilAcademicoPage`

---

## Persistencia local con SQLite

La aplicación utiliza `SQLiteAsyncConnection` para ejecutar las operaciones de la base de datos de forma asíncrona.

La base de datos se crea automáticamente con el nombre:

```text
registro_estudiantes.db3
```

El archivo se guarda en la carpeta de datos internos de la aplicación mediante una ruta basada en `FileSystem.AppDataDirectory`. Cada instalación de la aplicación mantiene su propia base de datos local.

Al iniciar el servicio académico:

1. Se abre o crea la base de datos.
2. Se crean las tablas necesarias.
3. Se comprueba si existen los datos académicos iniciales.
4. Solo se agregan los datos que todavía no estén registrados.

Esto permite conservar la información sin crear duplicados cada vez que se inicia sesión.

---

## Operaciones asíncronas

Las operaciones académicas utilizan `Task` y `async/await` para evitar bloqueos en la interfaz.

Entre los métodos principales se encuentran:

```text
CargarDatosDePruebaAsync
ObtenerEstudiantesAsync
AgregarEstudianteAsync
ActualizarEstudianteAsync
EliminarEstudianteAsync
ObtenerMateriasAsync
AgregarMateriaAsync
ActualizarMateriaAsync
EliminarMateriaAsync
ObtenerAsistenciasAsync
AgregarAsistenciaAsync
ActualizarAsistenciaAsync
EliminarAsistenciaAsync
ObtenerCalificacionesAsync
AgregarCalificacionAsync
ActualizarCalificacionAsync
EliminarCalificacionAsync
```

---

## CRUD persistente

La aplicación permite realizar las cuatro operaciones principales sobre SQLite:

| Operación | Descripción |
|---|---|
| Crear | Registrar información nueva desde los formularios |
| Consultar | Recuperar y mostrar los registros guardados |
| Actualizar | Modificar un registro existente y conservar sus cambios |
| Eliminar | Borrar un registro después de solicitar confirmación |

Las operaciones fueron implementadas y comprobadas en Estudiantes, Materias, Asistencias y Calificaciones.

---

## Formularios dinámicos

Cada módulo utiliza el mismo formulario para crear y editar registros.

Cuando no hay un registro seleccionado, el formulario trabaja en modo creación:

- Nuevo estudiante.
- Nueva materia.
- Nueva asistencia.
- Nueva calificación.

Cuando se selecciona una tarjeta, el formulario cambia al modo edición:

- Los campos se cargan automáticamente.
- El título del formulario cambia.
- El botón principal permite guardar los cambios.
- Se muestra una opción para cancelar la edición.
- El registro también puede eliminarse con confirmación.

---

## Módulo de Estudiantes

Permite administrar:

- Matrícula.
- Nombre.
- Apellido.
- Carrera.
- Teléfono.

Validaciones principales:

- Campos obligatorios.
- Formato de matrícula.
- Matrículas no duplicadas.
- Exclusión del propio registro durante la edición.
- Validación del teléfono.

La pantalla también incorpora un buscador que filtra estudiantes en tiempo real por matrícula, nombre, apellido o carrera.

---

## Módulo de Materias

Permite administrar:

- Código.
- Nombre de la asignatura.
- Profesor.
- Cantidad de créditos.

Validaciones principales:

- Campos obligatorios.
- Códigos de materia no duplicados.
- Exclusión del propio registro durante la edición.
- Créditos expresados como un número válido.

---

## Módulo de Asistencias

Permite administrar:

- Estudiante.
- Materia.
- Fecha.
- Estado de asistencia.

Estados disponibles:

- Presente.
- Ausente.
- Excusa.

Validaciones principales:

- Selección obligatoria de estudiante, materia y estado.
- La fecha no puede ser futura.
- No se permite repetir la combinación de estudiante, materia y fecha.
- El propio registro se excluye durante la edición.

También se corrigió el desplazamiento del formulario y se eliminó un separador que se mostraba como una barra negra inferior en Android.

---

## Módulo de Calificaciones

Permite administrar:

- Estudiante.
- Materia.
- Nota.
- Observación.

Validaciones principales:

- Selección obligatoria de estudiante y materia.
- Nota numérica entre 0 y 100.
- Observación de hasta 120 caracteres.
- Una calificación por estudiante y materia.
- Exclusión del propio registro durante la edición.

---

## Resumen estadístico de inicio

La pantalla principal presenta un resumen de la información almacenada:

- Cantidad de estudiantes.
- Cantidad de materias.
- Cantidad de asistencias.
- Cantidad de calificaciones.

Los valores se consultan desde SQLite y se actualizan según los registros existentes.

---

## Perfil académico del estudiante

La aplicación incorpora una pantalla de perfil académico individual que permite consultar de forma organizada la información relacionada con un estudiante.

El perfil presenta:

- Datos personales y académicos.
- Matrícula y carrera.
- Resumen de asistencias.
- Calificaciones registradas.
- Información vinculada a las materias.

Esta función permite reunir en una sola vista datos que se encuentran distribuidos entre diferentes tablas de SQLite.

---

## Restricciones y control de duplicados

Además de las validaciones visuales de los formularios, la base de datos contiene restricciones para proteger la integridad de la información.

Se impiden los siguientes duplicados:

- Dos estudiantes con la misma matrícula.
- Dos materias con el mismo código.
- Dos asistencias con el mismo estudiante, materia y fecha.
- Dos calificaciones con el mismo estudiante y materia.

El servicio captura las restricciones de SQLite y muestra mensajes comprensibles al usuario en lugar de cerrar la aplicación.

---

## Datos académicos iniciales

La aplicación incluye datos válidos para facilitar las pruebas:

### Estudiantes

- Junior Alexis Valera Rijo — `MT-2023-00518`.
- Yisel del Carmen Santana Rosario — `SD-18-11014`.

### Materia

- Programación de Aplicaciones — `INF-4316`.

Los datos iniciales se agregan únicamente cuando no existen, por lo que no se repiten al cerrar sesión o reiniciar la aplicación.

---

## Pruebas funcionales realizadas

La Unidad 6 fue validada manualmente en Android.

Para cada módulo se comprobó el siguiente flujo:

1. Crear un registro temporal.
2. Cerrar sesión y volver a iniciar.
3. Confirmar que el registro continuara almacenado.
4. Editar el registro temporal.
5. Reiniciar la sesión y comprobar que los cambios permanecieran.
6. Eliminar el registro temporal con confirmación.
7. Volver a iniciar y confirmar su eliminación definitiva.

Resultados validados:

- CRUD persistente de Estudiantes: correcto.
- CRUD persistente de Materias: correcto.
- CRUD persistente de Asistencias: correcto.
- CRUD persistente de Calificaciones: correcto.
- Datos iniciales sin duplicados: correcto.
- Desplazamiento e interacción de formularios: correcto.
- Persistencia después de cerrar sesión: correcta.
- Mensajes de guardado, actualización y eliminación: correctos.
- Confirmaciones antes de eliminar: correctas.

---

## Compilación y ejecución

### Requisitos

- Visual Studio 2022 con la carga de trabajo de .NET MAUI.
- SDK de .NET compatible con el proyecto.
- Android SDK y un emulador configurado.
- Git.

### Clonar el repositorio

```bash
git clone https://github.com/juniorvaleraofficial/Registro-de-Estudiantes.git
cd Registro-de-Estudiantes
git checkout unidad-6
```

### Restaurar dependencias

```powershell
dotnet restore .\RegistroEstudiantes.Mobile\RegistroEstudiantes.Mobile.csproj
```

### Compilar para Android

```powershell
dotnet build `
  .\RegistroEstudiantes.Mobile\RegistroEstudiantes.Mobile.csproj `
  -f net10.0-android
```

### Ejecutar en Android

Con el emulador iniciado:

```powershell
dotnet build `
  .\RegistroEstudiantes.Mobile\RegistroEstudiantes.Mobile.csproj `
  -f net10.0-android `
  -t:Run
```

Si se utiliza una arquitectura Android específica, puede ser necesario restaurar y compilar indicando el Runtime Identifier correspondiente.

---

## Control de versiones

El proyecto utiliza commits separados para documentar claramente cada avance, entre ellos:

- Preparación de modelos e identificadores para SQLite.
- Migración del servicio académico a operaciones asíncronas.
- Integración persistente de estudiantes.
- Integración persistente de materias.
- Integración persistente de asistencias.
- Integración persistente de calificaciones.
- Manejo de restricciones y duplicados.
- Mejoras de desplazamiento e interacción.
- Buscador de estudiantes.
- Resumen estadístico de inicio.
- Perfil académico individual.
- Correcciones visuales específicas de Android.

---

## Evolución del proyecto

| Rama | Avance principal |
|---|---|
| `unidad-2` | Estructura inicial, navegación y primeras pantallas |
| `unidad-4` | Registro y consulta de información académica |
| `unidad-5` | Edición, eliminación y CRUD completo en memoria |
| `unidad-6` | Persistencia SQLite, operaciones asíncronas y consultas académicas mejoradas |

---

## Estado actual

La rama `unidad-6` cuenta con:

- Compilación Android correcta.
- Base de datos SQLite funcional.
- CRUD persistente en los cuatro módulos.
- Interfaz validada en el emulador.
- Datos académicos iniciales conservados.
- Repositorio organizado mediante commits descriptivos.

El proyecto está preparado para la revisión y entrega correspondiente a la Unidad 6.

---

## Repositorio

[Registro de Estudiantes - rama unidad-6](https://github.com/juniorvaleraofficial/Registro-de-Estudiantes/tree/unidad-6)
