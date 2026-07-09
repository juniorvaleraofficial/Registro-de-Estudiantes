# Control Académico Estudiantil

Proyecto académico para la asignatura **INF-4316 - Programación de Aplicaciones Móviles**.

Este repositorio corresponde al proyecto móvil del **Grupo #6** y se trabaja por unidades mediante ramas de Git.

## Integrantes

- Junior Valera
- Yisel Santana

## Rama actual

```txt
unidad-4

# La rama unidad-4 corresponde a la Actividad 4 - MVVM y validaciones inline.

## Objetivo de la Unidad 4

Refactorizar los formularios principales de la aplicación para aplicar el patrón MVVM y mostrar validaciones detalladas debajo de cada campo, evitando mostrar los errores como ventanas emergentes.

La aplicación mantiene el enfoque académico de la unidad anterior, pero ahora separa mejor la lógica de la interfaz usando ViewModels, comandos y binding.

## Descripción de la aplicación

Control Académico Estudiantil es una aplicación móvil desarrollada con .NET MAUI para registrar y consultar información académica básica desde un dispositivo móvil.

## La app permite trabajar con las siguientes secciones:

Estudiantes
Materias
Asistencias
Calificaciones

## Los datos se manejan temporalmente en memoria mediante el servicio ServicioAcademico.

Funcionalidades principales
Login funcional con credenciales fijas.
Navegación entre pantallas después del login.
Menú lateral para acceder a los módulos.
Opción para cerrar sesión.
Uso del paquete CommunityToolkit.Mvvm.
ViewModel para cada entidad principal.
Formularios conectados por binding.
Botones conectados mediante commands.
Validaciones inline debajo de cada campo.
Validaciones específicas por módulo.
Alertas de éxito cuando el registro se guarda correctamente.
Limpieza automática del formulario después de guardar.
Listados actualizados con los nuevos registros.