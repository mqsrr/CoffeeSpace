{{- define "PaymentService.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | kebabcase}}
{{- end }}

{{- define "PaymentService.namespace" -}}
{{- default "default" .Values.global.namespaceOverride | trunc 63 | kebabcase}}
{{- end }}


{{- define "PaymentService.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{- end }}


{{- define "PaymentService.labels" -}}
helm.sh/chart: {{ include "PaymentService.chart" . }}
{{ include "PaymentService.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}


{{- define "PaymentService.selectorLabels" -}}
app.kubernetes.io/name: {{ include "PaymentService.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}