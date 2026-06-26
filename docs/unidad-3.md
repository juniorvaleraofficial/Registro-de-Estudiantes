# Unidad 3 - Login, navegación y CRUD básico

## Asignatura

**INF-4316 - Programación de Aplicaciones Móviles**

## Proyecto

**Control Académico Estudiantil**

## Grupo

**Grupo #6**

## Integrantes

- Junior Valera
- Yisel Santana

## Enunciado resumido

La Unidad 3 solicita implementar una aplicación móvil con login funcional, navegación entre cuatro pantallas y CRUD básico. Los datos deben guardarse solo en memoria, sin base de datos.

Además de cumplir los criterios técnicos, la aplicación debe representar una solución útil y realista de la vida diaria.

## Idea seleccionada

La aplicación será un **Control Académico Estudiantil**.

Esta idea permite que un profesor, coordinador o encargado académico pueda registrar información básica de estudiantes, materias, asistencias y calificaciones desde una aplicación móvil sencilla.

La propuesta es realista porque muchas personas todavía manejan este tipo de información en libretas, hojas de Excel o mensajes sueltos. Esta app organiza esos datos en pantallas claras y fáciles de usar.

## Objetivo general

Crear una aplicación móvil básica en .NET MAUI que permita iniciar sesión, navegar mediante un menú lateral y registrar información académica en memoria.

## Objetivos específicos

- Validar el acceso mediante credenciales fijas.
- Mostrar el menú lateral solo después del login.
- Permitir navegar entre las secciones principales de la aplicación.
- Crear cuatro entidades principales relacionadas con el control académico.
- Guardar los registros temporalmente en memoria.
- Validar campos obligatorios antes de guardar.
- Mostrar los datos creados en listados.

## Credenciales de prueba

```txt
Usuario: admin
Contraseña: 1234
```

## Módulos de la aplicación

## 1. Login

El login será la primera pantalla de la aplicación.

Debe permitir al usuario ingresar usuario y contraseña. Si los datos son correctos, se mostrará la pantalla principal con el menú lateral. Si son incorrectos, se mostrará un mensaje de error.

Validaciones mínimas:

- El usuario es obligatorio.
- La contraseña es obligatoria.
- Las credenciales deben coincidir con las credenciales fijas.

## 2. Inicio

Pantalla principal después del login.

Mostrará una bienvenida y una descripción breve del sistema. También puede mostrar un resumen simple de registros creados, por ejemplo:

- Total de estudiantes registrados.
- Total de materias registradas.
- Total de asistencias registradas.
- Total de calificaciones registradas.

## 3. Estudiantes

Permite registrar y listar estudiantes.

Campos propuestos:

- Matrícula
- Nombre
- Apellido
- Carrera
- Teléfono

Validaciones mínimas:

- Matrícula obligatoria.
- Nombre obligatorio.
- Apellido obligatorio.
- Carrera obligatoria.

Ejemplo de registro:

```txt
Matrícula: MT-2023-00518
Nombre: Junior
Apellido: Valera
Carrera: Ingeniería de Software
Teléfono: 809-000-0000
```

## 4. Materias

Permite registrar y listar materias.

Campos propuestos:

- Código
- Nombre
- Profesor
- Créditos

Validaciones mínimas:

- Código obligatorio.
- Nombre obligatorio.
- Profesor obligatorio.
- Créditos obligatorio.

Ejemplo de registro:

```txt
Código: INF-4316
Nombre: Programación de Aplicaciones Móviles
Profesor: Nombre del profesor
Créditos: 4
```

## 5. Asistencias

Permite registrar y listar asistencias.

Campos propuestos:

- Estudiante
- Materia
- Fecha
- Estado

Estados disponibles:

- Presente
- Ausente
- Excusa

Validaciones mínimas:

- Estudiante obligatorio.
- Materia obligatoria.
- Fecha obligatoria.
- Estado obligatorio.

Ejemplo de registro:

```txt
Estudiante: Junior Valera
Materia: Programación de Aplicaciones Móviles
Fecha: 2026-06-23
Estado: Presente
```

## 6. Calificaciones

Permite registrar y listar calificaciones.

Campos propuestos:

- Estudiante
- Materia
- Nota
- Observación

Validaciones mínimas:

- Estudiante obligatorio.
- Materia obligatorio.
- Nota obligatoria.
- La nota debe estar en un rango válido, por ejemplo de 0 a 100.

Ejemplo de registro:

```txt
Estudiante: Yisel Santana
Materia: Programación de Aplicaciones Móviles
Nota: 95
Observación: Buen desempeño en la práctica.
```

## Entidades del sistema

La aplicación tendrá cuatro modelos principales:

```txt
Estudiante
Materia
Asistencia
Calificacion
```

## Servicio en memoria

Para cumplir con el enunciado de la unidad, los datos se guardarán en memoria.

Servicio propuesto:

```txt
ServicioAcademico
```

Responsabilidades del servicio:

- Guardar estudiantes.
- Guardar materias.
- Guardar asistencias.
- Guardar calificaciones.
- Devolver los listados para mostrarlos en pantalla.
- Mantener los datos disponibles mientras la app esté abierta.

Cuando la aplicación se cierre, los datos se perderán. Esto es correcto para esta unidad porque todavía no se solicita base de datos.

## Navegación esperada

Después del login, el usuario podrá navegar mediante menú lateral.

Opciones del menú:

```txt
Inicio
Estudiantes
Materias
Asistencias
Calificaciones
Cerrar sesión
```

La opción **Cerrar sesión** debe regresar al login y ocultar el menú lateral.

## Criterios de aceptación cubiertos

| Criterio solicitado | Cómo se cubrirá |
|---|---|
| Login funcional con credenciales fijas | Pantalla Login con usuario `admin` y contraseña `1234` |
| Navegación entre 4 pantallas | Estudiantes, Materias, Asistencias y Calificaciones |
| Flyout solo después del login | El menú lateral se mostrará luego de iniciar sesión |
| Cerrar sesión | Opción en el menú lateral para volver al login |
| 4 entidades base | Estudiante, Materia, Asistencia y Calificacion |
| Servicio en memoria | ServicioAcademico con listas temporales |
| Formulario con validación mínima | Validación de campos obligatorios antes de guardar |
| Listado de registros creados | Cada módulo tendrá su listado |

## Capturas sugeridas para la entrega

Para la entrega final se deben tomar capturas de:

1. Login con credenciales incorrectas.
2. Login con credenciales correctas.
3. Pantalla principal después del login.
4. Menú lateral abierto.
5. Formulario de estudiantes vacío.
6. Formulario de estudiantes lleno.
7. Listado de estudiantes con registros creados.
8. Formulario de materias lleno.
9. Listado de materias.
10. Registro de asistencia.
11. Listado de asistencias.
12. Registro de calificación.
13. Listado de calificaciones.
14. Cierre de sesión.

## Formato de entrega sugerido

- URL del repositorio en la rama `unidad-3`.
- Capturas de funcionamiento.
- Documento breve explicativo.

## Estado del documento

Documento inicial de planificación para la Unidad 3.

Este documento debe actualizarse al finalizar la implementación con capturas, decisiones finales y cualquier ajuste realizado durante el desarrollo.
