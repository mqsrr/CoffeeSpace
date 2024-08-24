{{- define "IdentityApi.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | kebabcase}}
{{- end }}

{{- define "IdentityApi.namespace" -}}
{{- default "default" .Values.global.namespaceOverride | kebabcase}}
{{- end }}


{{- define "IdentityApi.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{- end }}


{{- define "IdentityApi.labels" -}}
helm.sh/chart: {{ include "IdentityApi.chart" . }}
{{ include "IdentityApi.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}


{{- define "IdentityApi.selectorLabels" -}}
app.kubernetes.io/name: {{ include "IdentityApi.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}