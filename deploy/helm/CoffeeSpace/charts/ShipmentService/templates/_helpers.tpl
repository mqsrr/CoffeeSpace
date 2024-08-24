{{- define "ShipmentService.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | kebabcase}}
{{- end }}

{{- define "ShipmentService.namespace" -}}
{{- default "default" .Values.global.namespaceOverride | trunc 63 | kebabcase}}
{{- end }}


{{- define "ShipmentService.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{- end }}


{{- define "ShipmentService.labels" -}}
helm.sh/chart: {{ include "ShipmentService.chart" . }}
{{ include "ShipmentService.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}


{{- define "ShipmentService.selectorLabels" -}}
app.kubernetes.io/name: {{ include "ShipmentService.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}