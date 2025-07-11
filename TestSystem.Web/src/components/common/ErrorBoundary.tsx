import {Component, ErrorInfo, ReactNode} from 'react'
import {Button, Card, CardBody} from '@heroui/react'
import {AlertTriangle, Home, RefreshCw} from 'lucide-react'

interface Props {
    children: ReactNode
    fallback?: ReactNode
}

interface State {
    hasError: boolean
    error?: Error
    errorInfo?: ErrorInfo
}

export class ErrorBoundary extends Component<Props, State> {
    constructor(props: Props) {
        super(props)
        this.state = {hasError: false}
    }

    static getDerivedStateFromError(error: Error): State {
        return {hasError: true, error}
    }

    componentDidCatch(error: Error, errorInfo: ErrorInfo) {
        console.error('Error caught by boundary:', error, errorInfo)
        this.setState({error, errorInfo})
    }

    handleRetry = () => {
        this.setState({hasError: false, error: undefined, errorInfo: undefined})
    }

    handleGoHome = () => {
        window.location.href = '/dashboard'
    }

    render() {
        if (this.state.hasError) {
            if (this.props.fallback) {
                return this.props.fallback
            }

            return (
                <div className="min-h-screen flex items-center justify-center bg-gray-50 dark:bg-gray-900 px-4">
                    <Card className="w-full max-w-md">
                        <CardBody className="text-center space-y-6 p-8">
                            <div className="flex justify-center">
                                <div className="p-4 bg-red-100 dark:bg-red-900/20 rounded-full">
                                    <AlertTriangle className="h-8 w-8 text-red-600 dark:text-red-400"/>
                                </div>
                            </div>

                            <div className="space-y-2">
                                <h1 className="text-xl font-semibold text-gray-900 dark:text-white">
                                    Something went wrong
                                </h1>
                                <p className="text-gray-600 dark:text-gray-400">
                                    We're sorry, but something unexpected happened. Please try again or return to the
                                    home page.
                                </p>
                            </div>

                            {process.env.NODE_ENV === 'development' && this.state.error && (
                                <details className="text-left text-xs bg-gray-100 dark:bg-gray-800 p-3 rounded border">
                                    <summary className="cursor-pointer font-medium mb-2">
                                        Error Details (Development)
                                    </summary>
                                    <pre className="whitespace-pre-wrap break-words">
                    {this.state.error.toString()}
                                        {this.state.errorInfo?.componentStack}
                  </pre>
                                </details>
                            )}

                            <div className="flex flex-col sm:flex-row gap-3">
                                <Button
                                    variant="flat"
                                    startContent={<RefreshCw className="h-4 w-4"/>}
                                    onPress={this.handleRetry}
                                    className="flex-1"
                                >
                                    Try Again
                                </Button>
                                <Button
                                    color="primary"
                                    startContent={<Home className="h-4 w-4"/>}
                                    onPress={this.handleGoHome}
                                    className="flex-1"
                                >
                                    Go Home
                                </Button>
                            </div>
                        </CardBody>
                    </Card>
                </div>
            )
        }

        return this.props.children
    }
}