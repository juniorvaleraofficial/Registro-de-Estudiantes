# Control Académico Estudiantil

Proyecto académico para la asignatura **INF-4316 - Programación de Aplicaciones Móviles**.

Este repositorio corresponde al proyecto móvil del **Grupo #6** y se trabaja por unidades mediante ramas de Git.

## Integrantes

- Junior Valera
- Yisel Santana

## Rama actual

```txt
unidad-3
```

La rama `unidad-3` corresponde a la **Actividad 3 - Login, navegación y CRUD básico**.

## Objetivo de la Unidad 3

Desarrollar una aplicación móvil útil y realista para el control académico básico, usando **.NET MAUI** y almacenamiento temporal en memoria.

La app estará orientada a un profesor, coordinador o encargado académico que necesite registrar y consultar información básica de estudiantes, materias, asistencias y calificaciones desde el celular.

## Descripción de la aplicación

**Control Académico Estudiantil** será una aplicación móvil con inicio de sesión, menú lateral y cuatro secciones principales:

1. Estudiantes
2. Materias
3. Asistencias
4. Calificaciones

La aplicación no usará base de datos en esta unidad. Los datos se guardarán temporalmente en memoria mientras la app esté en ejecución, de acuerdo con el enunciado de la actividad.

## Funcionalidades principales

- Login funcional con credenciales fijas.
- Validación de usuario y contraseña.
- Navegación entre pantallas después del login.
- Menú lateral visible solo después de iniciar sesión.
- Opción para cerrar sesión.
- Formularios de creación con validación mínima.
- Listados de registros creados.
- Servicio en memoria para almacenar los datos temporalmente.

## Credenciales de prueba

```txt
Usuario: admin
Contraseña: 1234
```

## Entidades del proyecto

Para cumplir con los criterios de la Unidad 3, la aplicación manejará cuatro entidades principales:

### Estudiante

Representa a un estudiante registrado en el sistema.

Campos propuestos:

- Matrícula
- Nombre
- Apellido
- Carrera
- Teléfono

### Materia

Representa una asignatura o materia académica.

Campos propuestos:

- Código
- Nombre
- Profesor
- Créditos

### Asistencia

Representa el registro de asistencia de un estudiante en una materia.

Campos propuestos:

- Estudiante
- Materia
- Fecha
- Estado

Estados sugeridos:

- Presente
- Ausente
- Excusa

### Calificación

Representa una nota asignada a un estudiante en una materia.

Campos propuestos:

- Estudiante
- Materia
- Nota
- Observación

## Pantallas previstas

### Login

Pantalla inicial donde el usuario ingresa sus credenciales.

### Inicio

Pantalla principal después del login, con resumen breve del sistema.

### Estudiantes

Formulario para crear estudiantes y listado de estudiantes registrados.

### Materias

Formulario para crear materias y listado de materias registradas.

### Asistencias

Formulario para registrar asistencia y listado de asistencias creadas.

### Calificaciones

Formulario para registrar calificaciones y listado de notas creadas.

## Servicio en memoria

La aplicación usará un servicio en memoria para manejar los registros durante la ejecución de la app.

Nombre sugerido:

```txt
ServicioAcademico
```

Este servicio administrará listas temporales para:

- Estudiantes
- Materias
- Asistencias
- Calificaciones

## Tecnologías

- .NET MAUI
- C#
- XAML
- Git
- GitHub

## Organización del repositorio

```txt
Registro-de-Estudiantes/
├── RegistroEstudiantes.Mobile/
├── RegistroEstudiantes.Api/
├── docs/
│   └── unidad-3.md
└── README.md
```

> Nota: Para esta unidad, el foco principal estará en la aplicación móvil. El backend creado en unidades anteriores puede permanecer en el repositorio, pero la Actividad 3 trabajará los datos en memoria, sin base de datos.

## Criterios de aceptación de la Unidad 3

- Implementar login funcional con credenciales fijas.
- Implementar navegación funcional entre las pantallas.
- Configurar el menú lateral para que solo aparezca después del login.
- Agregar la opción **Cerrar sesión**.
- Crear al menos cuatro entidades base.
- Crear un servicio en memoria para almacenar los registros.
- Implementar formularios de creación con validación mínima.
- Implementar listados que muestren los registros creados.

## Estado actual

Documentación inicial de la Unidad 3 creada en la rama `unidad-3`.

Próximo paso: implementar la estructura de login, navegación, modelos, servicio en memoria, formularios y listados dentro del proyecto móvil.
