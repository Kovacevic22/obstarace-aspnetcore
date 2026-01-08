interface Props {
    isOpen: boolean;
    onClose: () => void;
    onConfirm: () => void;
    error?: string | null;
    title: string;
    message: string;
    variant?: 'danger' | 'success';
}

export function ConfirmModal({ isOpen, onClose, onConfirm, error = null, title, message, variant = 'success' }: Props) {
    if (!isOpen) return null;

    const isDanger = variant === 'danger';
    const brandColor = isDanger ? 'border-red-600' : 'border-accent/40';
    const textColor = isDanger ? 'text-red-600' : 'text-accent';
    const btnBg = isDanger ? 'bg-red-600 hover:bg-red-500' : 'bg-accent hover:bg-white text-dark';
    const shadowColor = isDanger ? 'rgba(239,68,68,0.2)' : 'rgba(166,124,82,0.2)';

    return (
        <div className="fixed inset-0 z-200 flex items-center justify-center p-4 italic uppercase font-black">
            <div className="absolute inset-0 bg-black/80 backdrop-blur-sm" onClick={onClose} />

            <div className={`relative bg-[#1a2421] border-2 ${error ? 'border-red-600' : brandColor} p-8 max-w-md w-full shadow-[0_0_50px_${shadowColor}] transition-all`}>
                <h3 className={`text-2xl ${error ? 'text-red-600' : textColor} tracking-tighter mb-2 italic`}>
                    {error ? "SYSTEM ERROR" : title}
                </h3>
                <p className="text-white/60 text-xs mb-8 not-italic font-medium leading-relaxed lowercase first-letter:uppercase">
                    {error || message}
                </p>

                <div className="flex gap-4">
                    {!error ? (
                        <>
                            <button onClick={onConfirm} className={`flex-1 ${btnBg} py-4 transition-all cursor-pointer shadow-lg active:scale-95 uppercase`}>
                                [ {isDanger ? 'CONFIRM' : 'ACCEPT'} ]
                            </button>
                            <button onClick={onClose} className="flex-1 border border-white/10 text-white/40 hover:text-white py-4 transition-all cursor-pointer uppercase">
                                [ ABORT ]
                            </button>
                        </>
                    ) : (
                        <button onClick={onClose} className="flex-1 bg-white/10 text-white py-4 hover:bg-white/20 transition-all cursor-pointer uppercase">
                            [ RETURN_TO_BASE ]
                        </button>
                    )}
                </div>
            </div>
        </div>
    );
}
export default ConfirmModal;