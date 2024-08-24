{{- define "OrderingApi.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | kebabcase}}
{{- end }}

{{- define "OrderingApi.namespace" -}}
{{- default "default" .Values.global.namespaceOverride | trunc 63 | kebabcase}}
{{- end }}


{{- define "OrderingApi.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{- end }}


{{- define "OrderingApi.labels" -}}
helm.sh/chart: {{ include "OrderingApi.chart" . }}
{{ include "OrderingApi.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}


{{- define "OrderingApi.selectorLabels" -}}
app.kubernetes.io/name: {{ include "OrderingApi.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}