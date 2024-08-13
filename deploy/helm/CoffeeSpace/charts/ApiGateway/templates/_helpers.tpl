{{- define "ApiGateway.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | kebabcase}}
{{- end }}

{{- define "ApiGateway.namespace" -}}
{{- default "default" .Values.global.namespaceOverride | trunc 63 | kebabcase}}
{{- end }}


{{- define "ApiGateway.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{- end }}


{{- define "ApiGateway.labels" -}}
helm.sh/chart: {{ include "ApiGateway.chart" . }}
{{ include "ApiGateway.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}


{{- define "ApiGateway.selectorLabels" -}}
app.kubernetes.io/name: {{ include "ApiGateway.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}