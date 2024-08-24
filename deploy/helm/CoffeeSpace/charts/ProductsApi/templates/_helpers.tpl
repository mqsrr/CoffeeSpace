{{- define "ProductsApi.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | kebabcase}}
{{- end }}

{{- define "ProductsApi.namespace" -}}
{{- default "default" .Values.global.namespaceOverride | trunc 63 | kebabcase}}
{{- end }}


{{- define "ProductsApi.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{- end }}


{{- define "ProductsApi.labels" -}}
helm.sh/chart: {{ include "ProductsApi.chart" . }}
{{ include "ProductsApi.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}


{{- define "ProductsApi.selectorLabels" -}}
app.kubernetes.io/name: {{ include "ProductsApi.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}